using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteRequestDtos;
using RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteResponseDtos;
using RoadEye_Service.Services.GoogleApiService;
using RoadEye_Service.Services.RoadServices;
using RoadEye_Service.Types;
using System.Threading.Tasks;

namespace RoadEye_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestRouteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGoogleApiService _googleApiService;
        private readonly IRouteService _routeService;

        public SuggestRouteController(
            IMapper mapper,
            IGoogleApiService googleApiService,
            IRouteService routeService) {
            _mapper = mapper;
            _googleApiService = googleApiService;
            _routeService = routeService;
        }

        [HttpPost]
        public async Task<IActionResult> SuggestRoute(SuggestRouteRequestDto suggestRouteRequestDto) {
            if (suggestRouteRequestDto == null) {
                return BadRequest(new { error = "invalid payload" });
            }

            bool isRoadsIdsValid = await _googleApiService.FetchAndValidateRoadsByApiId(_mapper.Map<RoadIdsList>(suggestRouteRequestDto));
            if (!isRoadsIdsValid) {
                return UnprocessableEntity(new { error = "invalid one or more road id" });
            }

            SuggestRouteResponseDto suggestRouteResponseDto = _mapper.Map<SuggestRouteResponseDto>(suggestRouteRequestDto);
            return Ok(await _routeService.SetRouteSuggestionValues(suggestRouteResponseDto));
        }
    }
}