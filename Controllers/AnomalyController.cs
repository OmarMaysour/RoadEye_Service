using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using RoadEye_Service.Dtos.AnomalyDtos;
using RoadEye_Service.Models;
using RoadEye_Service.Repositories;
using RoadEye_Service.Services.ConstractPaginationHeader;
using RoadEye_Service.Services.GoogleApiService;
using RoadEye_Service.Services.RoadServices;
using RoadEye_Service.Types;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace RoadEye_Service.Controllers
{
    [Route("api/road")]
    [ApiController]
    public class AnomalyController : ControllerBase
    {
        private readonly IAnomalyRepository _anomalyRepository;
        private readonly IRoadRepository _roadRepository;
        private readonly IGoogleApiService _googleApiService;
        private readonly IHostingEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly IConstractPaginationHeaderService _constractPaginationHeaderService;
        private readonly IUpdateRoadConditionService _updateRoadConditionService;
        private readonly IAnomalyExistenceService _anomalyExistenceService;
        private readonly LinkGenerator _linkGenerator;

        public AnomalyController(IAnomalyRepository anomalyRepository,
            IRoadRepository roadRepository,
            IHostingEnvironment environment,
            IMapper mapper,
            IGoogleApiService googleApiService,
            IConstractPaginationHeaderService constractPaginationHeaderService,
            LinkGenerator linkGenerator,
            IUpdateRoadConditionService updateRoadConditionService,
            IAnomalyExistenceService anomalyExistenceService) {
            _anomalyRepository = anomalyRepository;
            _roadRepository = roadRepository;
            _environment = environment;
            _mapper = mapper;
            _googleApiService = googleApiService;
            _constractPaginationHeaderService = constractPaginationHeaderService;
            _linkGenerator = linkGenerator;
            _updateRoadConditionService = updateRoadConditionService;
            _anomalyExistenceService = anomalyExistenceService;
        }

        [HttpPost("anomaly")]
        public async Task<ActionResult> Post([ModelBinder(BinderType = typeof(JsonModelBinder))] CreateAnomalyDto createAnomalyDto, IFormFile AnomalyImage) {
            if (createAnomalyDto == null) {
                return BadRequest(new { error = "invalid anomaly payload" });
            } else if (AnomalyImage == null) {
                return BadRequest(new { error = "missing anomaly image" });
            } else if (AnomalyImage.Length > 10485760) {
                return StatusCode(StatusCodes.Status413PayloadTooLarge, new { error = "anomaly image too big" });
            } else if (!ModelState.IsValid) {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            Anomaly anomaly = _mapper.Map<Anomaly>(createAnomalyDto);

            Anomaly existAnomaly = await _anomalyExistenceService.AnomalyExistsAndUpdateDate(anomaly);
            if(existAnomaly != null) {
                return Ok(existAnomaly);
            }

            int fetchedRoadId = await _googleApiService.FetchAndValidateRoadByCoordinates(anomaly.Lat, anomaly.Lng);
            if (fetchedRoadId == 0) {
                return BadRequest(new { error = "invalid road coordinates" });
            } else {
                anomaly.RoadId = fetchedRoadId;
            }

            string tempFileName = Guid.NewGuid() + Path.GetExtension(AnomalyImage.FileName);
            anomaly.Frame_url = tempFileName;

            Anomaly createdAnomaly = await _anomalyRepository.Create(anomaly);
            if (createdAnomaly != null) {
                if (await SaveAnomalyImage(AnomalyImage, tempFileName)) {
                    _updateRoadConditionService.UpdateRoadCondition(createdAnomaly.Id);
                    return CreatedAtRoute("GetAnomaly", new { roadId = createdAnomaly.RoadId, id = createdAnomaly.Id }, createdAnomaly);
                } else {
                    _anomalyRepository.Delete(createdAnomaly);
                    return BadRequest(new { error = "invalid image" });
                }
            } else {
                return BadRequest(new { error = "invalid anomaly" });
            }
        }

        private async Task<bool> SaveAnomalyImage(IFormFile AnomalyImage, string tempFileName) {
            var uploads = Path.Combine(_environment.WebRootPath, "Images");
            if (AnomalyImage.Length > 0) {
                using (var fileStream = new FileStream(Path.Combine(uploads, tempFileName), FileMode.Create)) {
                    await AnomalyImage.CopyToAsync(fileStream);
                }
                return true;
            } else {
                return false;
            }
        }

        [HttpGet("{roadId}/anomaly/{id}", Name = "GetAnomaly")]
        public async Task<ActionResult> Get(int roadId, int id) {
            if (!await _roadRepository.IsRoadExists(roadId)) {
                return NotFound(new { error = "road not found" });
            }

            Anomaly anomaly = await _anomalyRepository.Get(roadId, id);
            if (anomaly != null) {
                return Ok(anomaly);
            } else {
                return NotFound(new { error = "anomaly not found" });
            }
        }

        [HttpGet("{roadId}/anomaly", Name = "GetAnomalies")]
        public async Task<ActionResult> Index(int roadId, [FromQuery] AnomalyIndexParameters anomalyIndexParameters) {
            if (!await _roadRepository.IsRoadExists(roadId)) {
                return NotFound(new { error = "road not found" });
            }

            PagedList<Anomaly> anomalies = await _anomalyRepository.PaginatedIndexForRoad(roadId, anomalyIndexParameters);
            ExpandoObject additionalQueryParameters = new ExpandoObject();
            (additionalQueryParameters as IDictionary<string, object>).Add("Type", anomalyIndexParameters.Type);

            Response.Headers.Add("X_Pagination",
                JsonConvert.SerializeObject(_constractPaginationHeaderService.ConstractPaginationHeader<Anomaly>(
                        anomalyIndexParameters, anomalies, GenerateResourceUri, additionalQueryParameters)));

            return Ok(anomalies);
        }

        public string GenerateResourceUri(ExpandoObject additionalQueryParameters) {
            return _linkGenerator.GetUriByAction(HttpContext, values: additionalQueryParameters);
        }
    }
}