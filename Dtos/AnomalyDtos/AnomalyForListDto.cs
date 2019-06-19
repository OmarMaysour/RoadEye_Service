using System;

namespace RoadEye_Service.Dtos.AnomalyDtos
{
    public class AnomalyForListDto
    {
        public int Id { get; set; }

        public string Frame_url { get; set; }

        public double Confidence { get; set; }

        public Geolocation Geolocation { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public String Type { get; set; }
    }
}