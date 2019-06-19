﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RoadEye_Service.Data;

namespace RoadEye_Service.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190515044058_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("RoadEye_Service.Models.Anomaly", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnomalyTypeId");

                    b.Property<double>("BumpConfidence");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2019, 5, 15, 6, 40, 56, 361, DateTimeKind.Local).AddTicks(2632));

                    b.Property<string>("Frame_url")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<double>("Lat");

                    b.Property<double>("Lng");

                    b.Property<double>("ManholeConfidence");

                    b.Property<double>("PotholeConfidence");

                    b.Property<int>("RoadId");

                    b.Property<double>("RumbleStripConfidence");

                    b.Property<DateTime>("TrueCreatedAt");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2019, 5, 15, 6, 40, 56, 366, DateTimeKind.Local).AddTicks(6434));

                    b.HasKey("Id");

                    b.HasIndex("AnomalyTypeId");

                    b.HasIndex("RoadId");

                    b.ToTable("Anomalies");
                });

            modelBuilder.Entity("RoadEye_Service.Models.AnomalyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AnomalyTypeName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("AnomalyTypeName")
                        .HasName("AlternateKey_AnomalyTypeName");

                    b.ToTable("AnomalyTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AnomalyTypeName = "Pothole"
                        },
                        new
                        {
                            Id = 2,
                            AnomalyTypeName = "Bump"
                        },
                        new
                        {
                            Id = 3,
                            AnomalyTypeName = "Manhole"
                        },
                        new
                        {
                            Id = 4,
                            AnomalyTypeName = "RumbleStrip"
                        });
                });

            modelBuilder.Entity("RoadEye_Service.Models.Road", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApiId")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2019, 5, 15, 6, 40, 56, 367, DateTimeKind.Local).AddTicks(13));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("RoadConditionTypeId");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2019, 5, 15, 6, 40, 56, 367, DateTimeKind.Local).AddTicks(378));

                    b.HasKey("Id");

                    b.HasIndex("RoadConditionTypeId");

                    b.ToTable("Roads");
                });

            modelBuilder.Entity("RoadEye_Service.Models.RoadConditionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConditionTypeName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("ConditionTypeName")
                        .HasName("AlternateKey_ConditionTypeName");

                    b.ToTable("RoadConditionTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConditionTypeName = "excellent"
                        },
                        new
                        {
                            Id = 2,
                            ConditionTypeName = "good"
                        },
                        new
                        {
                            Id = 3,
                            ConditionTypeName = "bad"
                        },
                        new
                        {
                            Id = 4,
                            ConditionTypeName = "undetermined"
                        });
                });

            modelBuilder.Entity("RoadEye_Service.Models.Anomaly", b =>
                {
                    b.HasOne("RoadEye_Service.Models.AnomalyType", "AnomalyType")
                        .WithMany("Anomalies")
                        .HasForeignKey("AnomalyTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RoadEye_Service.Models.Road", "Road")
                        .WithMany("Anomalies")
                        .HasForeignKey("RoadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RoadEye_Service.Models.Road", b =>
                {
                    b.HasOne("RoadEye_Service.Models.RoadConditionType", "RoadConditionType")
                        .WithMany("Roads")
                        .HasForeignKey("RoadConditionTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
