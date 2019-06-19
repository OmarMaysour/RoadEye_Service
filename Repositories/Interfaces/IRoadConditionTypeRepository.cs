using RoadEye_Service.Models;
using System.Threading.Tasks;

namespace RoadEye_Service.Repositories
{
    public interface IRoadConditionTypeRepository
    {
        Task<RoadConditionType> Get(int id);
    }
}
