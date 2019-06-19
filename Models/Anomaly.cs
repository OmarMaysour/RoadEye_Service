using System;
using System.ComponentModel.DataAnnotations;

namespace RoadEye_Service.Models
{
    public class Anomaly
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Frame_url { get; set; }

        [Required]
        public double PotholeConfidence { get; set; }

        [Required]
        public double BumpConfidence { get; set; }

        [Required]
        public double ManholeConfidence { get; set; }

        [Required]
        public double RumbleStripConfidence { get; set; }

        [Required]
        public double Lng { get; set; }

        [Required]
        public double Lat { get; set; }

        [Required]
        public DateTime TrueCreatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Required]
        public int RoadId { get; set; }
        public Road Road { get; set; }

        [Required]
        public int AnomalyTypeId { get; set; }
        public AnomalyType AnomalyType { get; set; }
    }
}
