using RoadEye_Service.Dtos.SuggestRouteDtos.SuggestRouteResponseDtos;
using RoadEye_Service.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RoadEye_Service.Services.RoadServices
{
    public class RouteService : IRouteService
    {
        public async Task<SuggestRouteResponseDto> SetRouteSuggestionValues(SuggestRouteResponseDto suggestRouteResponseDto) {
            int lowestScore = 99999;
            foreach (SuggestRouteResponseRouteDto route in suggestRouteResponseDto.Routes) {
                int bumpSum = 0, potholeSum = 0, manholeSum = 0, rumbleStrip = 0;
                foreach (Road road in route.Roads) {
                    foreach(Anomaly anomaly in road.Anomalies) {
                        switch (anomaly.AnomalyType.AnomalyTypeName) {
                            case "Bump":
                                bumpSum += 1;
                                break;
                            case "Pothole":
                                potholeSum += 1;
                                break;
                            case "Manhole":
                                manholeSum += 1;
                                break;
                            case "RumbleStrip":
                                rumbleStrip += 1;
                                break;
                        }
                    }
                }
                route.TotalAnomalies.Add("Bump", bumpSum);
                route.TotalAnomalies.Add("Pothole", potholeSum);
                route.TotalAnomalies.Add("Manhole", manholeSum);
                route.TotalAnomalies.Add("RumbleStrip", rumbleStrip);

                int score = (potholeSum * 10 + bumpSum * 5 + manholeSum * 8 + rumbleStrip * 3) / route.Roads.Count;
                if (score < 3) {
                    route.Condition = "Excellent";
                } else if (score > 3 && score < 15) {
                    route.Condition = "Good";
                } else {
                    route.Condition = "Bad";
                }

                if (score < lowestScore) {
                    lowestScore = score;
                    suggestRouteResponseDto.BestRoute = route.OriginalIndex;
                }
            }
            return suggestRouteResponseDto;
        }
    }
}