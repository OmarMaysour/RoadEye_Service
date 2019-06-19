using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace RoadEye_Service.Dtos.AnomalyDtos
{
    public class CreateAnomalyDto
    {
        [Required]
        public double PotholeConfidence { get; set; }

        [Required]
        public double BumpConfidence { get; set; }

        [Required]
        public double ManholeConfidence { get; set; }

        [Required]
        public double RumbleStripConfidence { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; }

        [Required]
        public double Lng { get; set; }

        [Required]
        public double Lat { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
