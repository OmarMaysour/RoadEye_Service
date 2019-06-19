using System.Collections.Generic;

namespace RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteRequestDtos
{
    public class SuggestRouteRequestDto
    {
        public ICollection<SuggestRouteRequestRouteDto> Routes { get; set; }
    }
}
