using Microsoft.EntityFrameworkCore;
using RoadEye_Service.Data;
using RoadEye_Service.Dtos.RoadDtos;
using RoadEye_Service.Models;
using RoadEye_Service.Services.GoogleApiService;
using RoadEye_Service.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadEye_Service.Repositories
{
    public class RoadRepository : IRoadRepository
    {
        private readonly DataContext _context;

        public RoadRepository(DataContext context) {
            _context = context;
        }

        public async Task<Road> Create(Road road) {
            await _context.AddAsync(road);
            await _context.SaveChangesAsync();

            return await Get(road.Id);
        }

        public async Task<Road> Get(int id) {
            return await _context.Roads.Include(r => r.RoadConditionType).Include(r => r.Anomalies).ThenInclude(r => r.AnomalyType).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Road> GetByApiId(string id) {
            return await _context.Roads.Include(r => r.Anomalies).FirstOrDefaultAsync(x => x.ApiId == id);
        }

        public Road GetByApiIdSync(string id) {
            return _context.Roads.Include(r => r.RoadConditionType).Include(r => r.Anomalies).ThenInclude(a => a.AnomalyType).FirstOrDefault(x => x.ApiId == id);
        }

        public async Task<PagedList<Road>> Index(RoadIndexParameters roadIndexParameters) {
            var collectionBeforePaging = _context.Roads.Include(r => r.RoadConditionType).Include(r => r.Anomalies).ThenInclude(r => r.AnomalyType).AsQueryable();

            if(!string.IsNullOrEmpty(roadIndexParameters.Condition)) {
                string conditionForWhereClause = roadIndexParameters.Condition.Trim();
                collectionBeforePaging = collectionBeforePaging.Where(r => r.RoadConditionType.ConditionTypeName.Equals(conditionForWhereClause, System.StringComparison.InvariantCultureIgnoreCase));
            }
         
            return await PagedList<Road>.Create(collectionBeforePaging,
                roadIndexParameters.PageNumber,
                roadIndexParameters.PageSize);
        }

        public async Task<bool> IsRoadExists(int id) {
            return await _context.Roads.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> IsRoadExistsByApiId(string id) {
            return await _context.Roads.AnyAsync(x => x.ApiId == id);
        }


        public async Task<Road> UpdateRoadCondition(int roadId, int newConditionId) {
            Road road = await _context.Roads.FirstOrDefaultAsync(x => x.Id == roadId);
            if (road != null) {
                road.RoadConditionTypeId = newConditionId;
                _context.Roads.Update(road);
                _context.SaveChanges();
            } else {
                return null;
            }
            return road;
        }

        public async Task<bool> Save() {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}