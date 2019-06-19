using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RoadEye_Service.Models;
using System;
using System.Collections.Generic;

namespace RoadEye_Service.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
           : base(options) {
            Database.Migrate();
        }

        public DbSet<Anomaly> Anomalies { get; set; }
        public DbSet<Road> Roads { get; set; }
        public DbSet<AnomalyType> AnomalyTypes { get; set; }
        public DbSet<RoadConditionType> RoadConditionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<AnomalyType>().HasAlternateKey(a => a.AnomalyTypeName).HasName("AlternateKey_AnomalyTypeName");
            modelBuilder.Entity<RoadConditionType>().HasAlternateKey(c => c.ConditionTypeName).HasName("AlternateKey_ConditionTypeName");

            modelBuilder.Entity<AnomalyType>().HasData(
                new AnomalyType {
                    Id = 1,
                    AnomalyTypeName = "Pothole"
                },
                new AnomalyType {
                    Id = 2,
                    AnomalyTypeName = "Bump"
                },
                new AnomalyType {
                    Id = 3,
                    AnomalyTypeName = "Manhole"
                },
                new AnomalyType {
                    Id = 4,
                    AnomalyTypeName = "RumbleStrip"
                }
            );

            modelBuilder.Entity<RoadConditionType>().HasData(
                new RoadConditionType {
                    Id = 1,
                    ConditionTypeName = "excellent"
                },
                new RoadConditionType {
                    Id = 2,
                    ConditionTypeName = "good"
                },
                new RoadConditionType {
                    Id = 3,
                    ConditionTypeName = "bad"
                },
                new RoadConditionType {
                    Id = 4,
                    ConditionTypeName = "undetermined"
                }
            );

            modelBuilder.Entity<Anomaly>().Property(a => a.CreatedAt).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Anomaly>().Property(a => a.UpdatedAt).HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Road>().Property(r => r.CreatedAt).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Road>().Property(r => r.UpdatedAt).HasDefaultValue(DateTime.Now);
        }
    }
}