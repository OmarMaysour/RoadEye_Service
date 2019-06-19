using RoadEye_Service.Models;
using RoadEye_Service.Repositories;
using RoadEye_Service.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadEye_Service.Services.GoogleApiService
{
    public class GoogleApiService : IGoogleApiService
    {
        private readonly IGoogleAPIRequestsManager _googlePlaceAPIRequestsManager;
        private readonly IRoadRepository _roadRepository;
        private readonly IRoadConditionTypeRepository _roadConditionTypeRepository;

        public GoogleApiService(
            IGoogleAPIRequestsManager googlePlaceAPIRequestsManager
            , IRoadRepository roadRepository
            , IRoadConditionTypeRepository roadConditionTypeRepository) {
            _googlePlaceAPIRequestsManager = googlePlaceAPIRequestsManager;
            _roadRepository = roadRepository;
            _roadConditionTypeRepository = roadConditionTypeRepository;
        }

        public async Task<bool> FetchAndValidateRoadsByApiId(RoadIdsList roadIdsList) {
            List<string> nonExistRoads = await GetNonExistRoads(roadIdsList.RoadIds);

            foreach(string roadId in nonExistRoads) {
                if(await DownloadAndSaveRoad(roadId) == null) {
                    return false;
                }
            }
            return true;
        }

        private async Task<List<string>> GetNonExistRoads(List<string> roadIds) {
            List<string> nonExistRoads = new List<string>();
            foreach (string roadId in roadIds) {
                if (!await _roadRepository.IsRoadExistsByApiId(roadId)) {
                    nonExistRoads.Add(roadId);
                }
            }
            return nonExistRoads;
        }

        public async Task<int> FetchAndValidateRoadByCoordinates(double lat, double lng) {
            ApiResponse fetchRoadResponse = await _googlePlaceAPIRequestsManager.GetPlaceIdByCoordinates(lat, lng);
            if (fetchRoadResponse.StatusCode != 200) {
                return 0;
            }

            string roadPlaceId = _googlePlaceAPIRequestsManager.ParseGetPlaceIdByCoordinates(fetchRoadResponse);

            Road road = await _roadRepository.GetByApiId(roadPlaceId);

            if (road == null) {
                road = await DownloadAndSaveRoad(roadPlaceId);
            }

            return road.Id;
        }

        private async Task<Road> DownloadAndSaveRoad(string apiId) {
            ApiResponse apiResponse = await _googlePlaceAPIRequestsManager.GetRoadByPlaceId(apiId);
            if (apiResponse.StatusCode != 200) {
                return null;
            } else {
                Road downloadedRoad = _googlePlaceAPIRequestsManager.ParseGooglePlaceAPIResponseToRoad(apiResponse.ResponseString);
                downloadedRoad.RoadConditionTypeId = 4;
                downloadedRoad.RoadConditionType = await _roadConditionTypeRepository.Get(4);
                await _roadRepository.Create(downloadedRoad);
                return downloadedRoad;
            }
        }
    }
}
