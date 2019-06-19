using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoadEye_Service.Models
{
    public class Road
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApiId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<Anomaly> Anomalies { get; set; } = new List<Anomaly>();

        [Required]
        public int RoadConditionTypeId { get; set; }
        public RoadConditionType RoadConditionType { get; set; }
    }
}
