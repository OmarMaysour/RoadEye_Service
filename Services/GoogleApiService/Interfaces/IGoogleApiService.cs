using RoadEye_Service.Types;
using System.Threading.Tasks;

namespace RoadEye_Service.Services.GoogleApiService
{
    public interface IGoogleApiService
    {
        Task<int> FetchAndValidateRoadByCoordinates(double lat, double lng);
        Task<bool> FetchAndValidateRoadsByApiId(RoadIdsList roadIdsList);
    }
}
