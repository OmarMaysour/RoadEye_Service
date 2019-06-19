using RoadEye_Service.Models;
using System.Collections.Generic;

namespace RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteResponseDtos
{
    public class SuggestRouteResponseRouteDto
    {
        public ICollection<Road> Roads { get; set; }
            = new List<Road>();

        public int OriginalIndex { get; set; }

        public string Condition { get; set; }

        public Dictionary<string, int> TotalAnomalies { get; set; } = new Dictionary<string, int>();
    }
}