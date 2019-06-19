using System.Collections.Generic;

namespace RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteRequestDtos
{
    public class SuggestRouteRequestRouteDto
    {
        public ICollection<string> RoadIds { get; set; }
        public int OriginalIndex { get; set; }
    }
}
