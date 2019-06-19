using Newtonsoft.Json.Linq;
using RoadEye_Service.Models;
using RoadEye_Service.Types;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoadEye_Service.Services.GoogleApiService
{
    public class GoogleAPIRequestsManager : IGoogleAPIRequestsManager
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string PLACE_API_BASE_URL = "https://maps.googleapis.com/maps/api/place/details/json";
        private const string ROADS_API_BASE_URL = "https://roads.googleapis.com/v1/nearestRoads";
        private const string KEY = "AIzaSyD67ivLMCvnq-CYF3kij48g3R4-Xb3ZWIc";

        public GoogleAPIRequestsManager(IHttpClientFactory clientFactory) {
            _clientFactory = clientFactory;
        }

        public async Task<ApiResponse> GetRoadByPlaceId(string id) {
            return await ExecuteHttpRequest(new HttpRequestMessage(HttpMethod.Get,
                PLACE_API_BASE_URL + "?key=" + KEY + "&placeid=" + id));
        }

        public async Task<ApiResponse> GetPlaceIdByCoordinates(double lat, double lng) {
            return await ExecuteHttpRequest(new HttpRequestMessage(HttpMethod.Get,
               ROADS_API_BASE_URL + "?key=" + KEY + "&points=" + lat + "," + lng));
        }

        private async Task<ApiResponse> ExecuteHttpRequest(HttpRequestMessage httpRequestMessage) {
            var request = httpRequestMessage;

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            string responseString = await response.Content.ReadAsStringAsync();
            int statusCode = (int)response.StatusCode;

            ApiResponse reponseValues = new ApiResponse {
                StatusCode = statusCode,
                ResponseString = responseString
            };

            return reponseValues;
        }

        public string ParseGetPlaceIdByCoordinates(ApiResponse apiResponse) {
            var parrsedJson = ((dynamic)JObject.Parse(apiResponse.ResponseString));
            return parrsedJson.snappedPoints[0].placeId;
        }

        public Road ParseGooglePlaceAPIResponseToRoad(string GooglePlaceAPIResponse) {
            Road road = new Road();

            dynamic JsonToParse = JObject.Parse(GooglePlaceAPIResponse);
            road.Name = JsonToParse.result.name;
            road.ApiId = JsonToParse.result.place_id;

            return road;
        }

    }
}