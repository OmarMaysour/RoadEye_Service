using Microsoft.EntityFrameworkCore;
using RoadEye_Service.Data;
using RoadEye_Service.Models;
using RoadEye_Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadEye_Service.Services.RoadServices
{
    public class AnomalyExistenceService: IAnomalyExistenceService
    {
        private readonly DataContext _context;
        private readonly IAnomalyRepository _anomalyRepository;

        public AnomalyExistenceService(DataContext context, IAnomalyRepository anomalyRepository) {
            _context = context;
            _anomalyRepository = anomalyRepository;
        }

        public async Task<Anomaly> AnomalyExistsAndUpdateDate(Anomaly anomaly) {
            Anomaly existAnomaly = await _context.Anomalies.FirstOrDefaultAsync(a => a.Lat <= anomaly.Lat + 0.001 && a.Lat >= anomaly.Lat - 0.001 
                                                                                && a.Lng <= anomaly.Lng + 0.001 && a.Lng >= anomaly.Lng - 0.001);
            if (existAnomaly != null) {
                await _anomalyRepository.UpdateAnomalyUpdatedAt(existAnomaly.Id, anomaly.CreatedAt);
                return existAnomaly;
            } else {
                return null;
            }
        }
    }
}
