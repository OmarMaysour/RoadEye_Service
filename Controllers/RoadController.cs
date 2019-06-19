using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using RoadEye_Service.Dtos.RoadDtos;
using RoadEye_Service.Models;
using RoadEye_Service.Repositories;
using RoadEye_Service.Services.ConstractPaginationHeader;
using RoadEye_Service.Types;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace RoadEye_Service.Controllers
{
    [ApiController]
    [Route("api/road")]
    public class RoadController : ControllerBase
    {
        private readonly IRoadRepository _roadRepository;
        private readonly IMapper _mapper;
        private readonly IConstractPaginationHeaderService _constractPaginationHeaderService;
        private readonly LinkGenerator _linkGenerator;

        public RoadController(IRoadRepository roadRepository, 
            IMapper mapper, 
            IConstractPaginationHeaderService constractPaginationHeaderService, 
            LinkGenerator linkGenerator) {
            _roadRepository = roadRepository;
            _mapper = mapper;
            _constractPaginationHeaderService = constractPaginationHeaderService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id) {
            Road road = await _roadRepository.Get(id);
            if (road != null) {
                return Ok(road);
            } else {
                return NotFound(new { error = "road not found" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] RoadIndexParameters roadIndexParameters) {
            PagedList<Road> roads = await _roadRepository.Index(roadIndexParameters);

            ExpandoObject additionalQueryParameters = new ExpandoObject();
            (additionalQueryParameters as IDictionary<string, object>).Add("Condition", roadIndexParameters.Condition);

            Response.Headers.Add("X_Pagination",
                JsonConvert.SerializeObject(_constractPaginationHeaderService.ConstractPaginationHeader<Road>(
                        roadIndexParameters, roads, GenerateResourceUri, additionalQueryParameters)));

            return Ok(roads);
        }

        public string GenerateResourceUri(ExpandoObject additionalQueryParameters) {
            return _linkGenerator.GetUriByAction(HttpContext, values: additionalQueryParameters);
        }
    }
}