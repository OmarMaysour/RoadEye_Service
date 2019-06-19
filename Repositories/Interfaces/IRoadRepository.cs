using System.Collections.Generic;
using System.Threading.Tasks;
using RoadEye_Service.Dtos.RoadDtos;
using RoadEye_Service.Models;
using RoadEye_Service.Types;

namespace RoadEye_Service.Repositories
{
    public interface IRoadRepository
    {
        Task<PagedList<Road>> Index(RoadIndexParameters roadIndexParameters);
        Task<Road> Create(Road road);
        Task<Road> Get(int id);
        Task<Road> GetByApiId(string id);
        Road GetByApiIdSync(string id);
        Task<bool> Save();
        Task<bool> IsRoadExists(int id);
        Task<bool> IsRoadExistsByApiId(string id);
        Task<Road> UpdateRoadCondition(int roadId, int newConditionId);
    }
}