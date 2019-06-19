using Microsoft.EntityFrameworkCore;
using RoadEye_Service.Data;
using RoadEye_Service.Models;
using System;
using System.Threading.Tasks;

namespace RoadEye_Service.Repositories
{
    public class AnomalyTypeRepository : IAnomalyTypeRepository
    {
        private readonly DataContext _context;

        public AnomalyTypeRepository(DataContext context) {
            _context = context;
        }

        public async Task<AnomalyType> Get(int id) {
            return await _context.AnomalyTypes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public AnomalyType GetByTypeNameSync(string typeName) {
            return _context.AnomalyTypes.FirstOrDefaultAsync(x => x.AnomalyTypeName == typeName).Result;
        }
    }
}
