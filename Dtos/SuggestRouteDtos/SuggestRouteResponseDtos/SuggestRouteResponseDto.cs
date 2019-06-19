using System.Collections.Generic;

namespace RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteResponseDtos
{
    public class SuggestRouteResponseDto
    {
        public ICollection<SuggestRouteResponseRouteDto> Routes { get; set; } = new List<SuggestRouteResponseRouteDto>();

        public int BestRoute { get; set; }
    }
}