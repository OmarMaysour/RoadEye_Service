using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoadEye_Service.Models
{
    public class RoadConditionType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ConditionTypeName { get; set; }

        [JsonIgnore]
        public ICollection<Road> Roads { get; set; } = new List<Road>();
    }
}
