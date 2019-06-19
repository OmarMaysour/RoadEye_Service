using RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteResponseDtos;
using System.Threading.Tasks;

namespace RoadEye_Service.Services.RoadServices
{
    public interface IRouteService
    {
        Task<SuggestRouteResponseDto> SetRouteSuggestionValues(SuggestRouteResponseDto suggestRouteResponseDto);
    }
}