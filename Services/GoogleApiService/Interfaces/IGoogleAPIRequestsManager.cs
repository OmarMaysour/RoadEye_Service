using RoadEye_Service.Models;
using RoadEye_Service.Types;
using System.Threading.Tasks;

namespace RoadEye_Service.Services.GoogleApiService
{
    public interface IGoogleAPIRequestsManager
    {
        Task<ApiResponse> GetRoadByPlaceId(string id);
        Task<ApiResponse> GetPlaceIdByCoordinates(double lat, double lng);
        string ParseGetPlaceIdByCoordinates(ApiResponse apiResponse);
        Road ParseGooglePlaceAPIResponseToRoad(string GooglePlaceAPIResponse);
    }
}
