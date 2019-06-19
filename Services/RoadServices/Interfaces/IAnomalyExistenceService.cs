using RoadEye_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadEye_Service.Services.RoadServices
{
    public interface IAnomalyExistenceService
    {
        Task<Anomaly> AnomalyExistsAndUpdateDate(Anomaly anomaly);
    }
}
