using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RoadEye_Service.Models;

namespace RoadEye_Service.Data
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }
        
        public void SeedRoads(){
            if (!_context.Roads.Any())
            {
                var roadsData = System.IO.File.ReadAllText("Data/SeedData/RoadSeedData.json");
                var roadsDeserialized = JsonConvert.DeserializeObject<List<Road>>(roadsData);

                foreach (var road in roadsDeserialized)
                {
                    _context.Roads.Add(road);
                }

                _context.SaveChanges();
            }
        }

        public void SeedAnomalies(){
            if (!_context.Anomalies.Any())
            {
                Anomaly[] anomalies = new Anomaly[20];
                for (int i = 0; i < 20; i++) {
                    Random rnd = new Random();
                    int type = rnd.Next(1, 4);
                    int roadId = rnd.Next(1, 27);
                    if (type == 1) {
                        anomalies[i] = new Anomaly {
                            Frame_url = "Images/pothole.jpg",
                            PotholeConfidence = 1,
                            BumpConfidence = 0,
                            ManholeConfidence = 0,
                            RumbleStripConfidence = 0,
                            Lng = 31.3396899,
                            Lat = 30.050421,
                            TrueCreatedAt = DateTime.Now,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            RoadId = roadId,
                            AnomalyTypeId = 1
                        };
                    } else if (type == 2) {
                        anomalies[i] = new Anomaly {
                            Frame_url = "Images/bump.jpg",
                            PotholeConfidence = 0,
                            BumpConfidence = 1,
                            ManholeConfidence = 0,
                            RumbleStripConfidence = 0,
                            Lng = 31.3396899,
                            Lat = 30.050421,
                            TrueCreatedAt = DateTime.Now,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            RoadId = roadId,
                            AnomalyTypeId = 2
                        };
                    } else if (type == 3) {
                        anomalies[i] = new Anomaly {
                            Frame_url = "Images/manhole.jpg",
                            PotholeConfidence = 0,
                            BumpConfidence = 0,
                            ManholeConfidence = 1,
                            RumbleStripConfidence = 0,
                            Lng = 31.3396899,
                            Lat = 30.050421,
                            TrueCreatedAt = DateTime.Now,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            RoadId = roadId,
                            AnomalyTypeId = 3
                        };
                    } else {
                        anomalies[i] = new Anomaly {
                            Frame_url = "Images/rumbleStrip.jpg",
                            PotholeConfidence = 0,
                            BumpConfidence = 0,
                            ManholeConfidence = 0,
                            RumbleStripConfidence = 1,
                            Lng = 31.3396899,
                            Lat = 30.050421,
                            TrueCreatedAt = DateTime.Now,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            RoadId = roadId,
                            AnomalyTypeId = 4
                        };
                    }
                }

                foreach (var anomaly in anomalies)
                {
                    _context.Anomalies.Add(anomaly);
                }

                _context.SaveChanges();
            }
        }
    }
}