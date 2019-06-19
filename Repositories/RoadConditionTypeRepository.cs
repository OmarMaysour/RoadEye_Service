using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoadEye_Service.Data;
using RoadEye_Service.Models;

namespace RoadEye_Service.Repositories
{
    public class RoadConditionTypeRepository : IRoadConditionTypeRepository
    {
        private readonly DataContext _context;

        public RoadConditionTypeRepository(DataContext context) {
            _context = context;
        }
        public async Task<RoadConditionType> Get(int id) {
            return await _context.RoadConditionTypes.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
