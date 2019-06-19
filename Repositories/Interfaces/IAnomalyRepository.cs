using RoadEye_Service.Dtos.AnomalyDtos;
using RoadEye_Service.Models;
using RoadEye_Service.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadEye_Service.Repositories
{
    public interface IAnomalyRepository
    {
        Task<PagedList<Anomaly>> PaginatedIndex(AnomalyIndexParameters anomalyIndexParameters);
        Task<PagedList<Anomaly>> PaginatedIndexForRoad(int roadId, AnomalyIndexParameters anomalyIndexParameters);
        Task<List<Anomaly>> IndexForRoad(int roadId);
        Task<Anomaly> Create(Anomaly anomaly);
        Task<Anomaly> Get(int roadId, int id);
        void Delete(Anomaly anomaly);
        Task<bool> Save();
        Task<bool> IsAnomalyExists(int id);
        Task<Anomaly> UpdateAnomalyUpdatedAt(int anomalyId, DateTime newUpdatedAt);
    }
}