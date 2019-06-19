using Microsoft.EntityFrameworkCore;
using RoadEye_Service.Data;
using RoadEye_Service.Dtos.AnomalyDtos;
using RoadEye_Service.Models;
using RoadEye_Service.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadEye_Service.Repositories
{
    public class AnomalyRepository : IAnomalyRepository
    {
        private readonly DataContext _context;

        public AnomalyRepository(DataContext context) {
            _context = context;
        }

        public async Task<Anomaly> Create(Anomaly anomaly) {
            await _context.AddAsync(anomaly);
            await _context.SaveChangesAsync();

            return await Get(anomaly.RoadId, anomaly.Id);
        }

        public void Delete(Anomaly anomaly) {
            _context.Remove(anomaly);
        }

        public async Task<Anomaly> Get(int roadId, int id) {
            return await _context.Anomalies.Include(a => a.AnomalyType).Include(a => a.Road).ThenInclude(r => r.RoadConditionType).FirstOrDefaultAsync(x => x.Id == id && x.RoadId == roadId);
        }

        public async Task<PagedList<Anomaly>> PaginatedIndex(AnomalyIndexParameters anomalyIndexParameters) {
            var collectionBeforePaging = _context.Anomalies.Include(x => x.AnomalyType).Include(x => x.Road).ThenInclude(r => r.RoadConditionType).AsQueryable();

            if(!string.IsNullOrEmpty(anomalyIndexParameters.Type)) {
                string typeForWhereClause = anomalyIndexParameters.Type.Trim();
                collectionBeforePaging = collectionBeforePaging.Where(a => a.AnomalyType.AnomalyTypeName.Equals(typeForWhereClause, System.StringComparison.InvariantCultureIgnoreCase));
            }

            return await PagedList<Anomaly>.Create(collectionBeforePaging,
                anomalyIndexParameters.PageNumber,
                anomalyIndexParameters.PageSize);
        }

        public async Task<PagedList<Anomaly>> PaginatedIndexForRoad(int roadId, AnomalyIndexParameters anomalyIndexParameters) {
            var collectionBeforePaging = _context.Anomalies.Include(x => x.AnomalyType).Where(x => x.RoadId == roadId);

            if (!string.IsNullOrEmpty(anomalyIndexParameters.Type)) {
                string typeForWhereClause = anomalyIndexParameters.Type.Trim();
                collectionBeforePaging = collectionBeforePaging.Where(a => a.AnomalyType.AnomalyTypeName.Equals(typeForWhereClause, System.StringComparison.InvariantCultureIgnoreCase));
            }

            return await PagedList<Anomaly>.Create(collectionBeforePaging,
                anomalyIndexParameters.PageNumber,
                anomalyIndexParameters.PageSize);
        }

        public async Task<List<Anomaly>> IndexForRoad(int roadId) {
            return await _context.Anomalies.Include(a => a.AnomalyType).Where(r => r.RoadId == roadId).ToListAsync();
        }

        public async Task<bool> Save() {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsAnomalyExists(int id) {
            return await _context.Anomalies.AnyAsync(x => x.Id == id);
        }

        public async Task<Anomaly> UpdateAnomalyUpdatedAt(int anomalyId, DateTime newUpdatedAt) {
            Anomaly anomaly = await _context.Anomalies.FirstOrDefaultAsync(x => x.Id == anomalyId);
            if (anomaly != null) {
                anomaly.UpdatedAt = newUpdatedAt;
                _context.Anomalies.Update(anomaly);
                _context.SaveChanges();
            } else {
                return null;
            }
            return anomaly;
        }
    }
}