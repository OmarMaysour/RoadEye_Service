using RoadEye_Service.Models;
using RoadEye_Service.Repositories;
using System.Collections.Generic;

namespace RoadEye_Service.Services.RoadServices
{
    public class UpdateRoadConditionService : IUpdateRoadConditionService
    {
        private readonly IAnomalyRepository _anomalyRepository;
        private readonly IRoadRepository _roadRepository;

        public UpdateRoadConditionService(IAnomalyRepository anomalyRepository, IRoadRepository roadRepository) {
            _anomalyRepository = anomalyRepository;
            _roadRepository = roadRepository;
        }

        public async void UpdateRoadCondition(int roadId) {
            List<Anomaly> anomalies = await _anomalyRepository.IndexForRoad(roadId);
            int score = CalculateScore(anomalies);
            if (score < 3) {
                await _roadRepository.UpdateRoadCondition(roadId, 1);
            } else if (score > 3 && score < 15) {
                await _roadRepository.UpdateRoadCondition(roadId, 2);
            } else {
                await _roadRepository.UpdateRoadCondition(roadId, 3);
            }
        }

        private int CalculateScore(List<Anomaly> anomalies) {
            int score = 0;
            foreach(Anomaly anomaly in anomalies) {
                switch(anomaly.AnomalyType.AnomalyTypeName) {
                    case "Pothole":
                        score += 10;
                        break;
                    case "Bump":
                        score += 5;
                        break;
                    case "Manhole":
                        score += 8;
                        break;
                    case "RumbleStrip":
                        score += 3;
                        break;
                    default:
                        score += 0;
                        break;
                }
            }
            return score;
        }
    }
}
