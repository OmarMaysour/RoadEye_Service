using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoadEye_Service.Models
{
    public class AnomalyType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AnomalyTypeName { get; set; }

        [JsonIgnore]
        public ICollection<Anomaly> Anomalies { get; set; } = new List<Anomaly>();
    }
}
