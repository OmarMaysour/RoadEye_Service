using RoadEye_Service.Models;
using System.Threading.Tasks;

namespace RoadEye_Service.Repositories
{
    public interface IAnomalyTypeRepository
    {
        Task<AnomalyType> Get(int id);
        AnomalyType GetByTypeNameSync(string typeName);
    }
}
