using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace RoadEye_Service.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnomalyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AnomalyTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnomalyTypes", x => x.Id);
                    table.UniqueConstraint("AlternateKey_AnomalyTypeName", x => x.AnomalyTypeName);
                });

            migrationBuilder.CreateTable(
                name: "RoadConditionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ConditionTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadConditionTypes", x => x.Id);
                    table.UniqueConstraint("AlternateKey_ConditionTypeName", x => x.ConditionTypeName);
                });

            migrationBuilder.CreateTable(
                name: "Roads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ApiId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 5, 15, 6, 40, 56, 367, DateTimeKind.Local).AddTicks(13)),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 5, 15, 6, 40, 56, 367, DateTimeKind.Local).AddTicks(378)),
                    RoadConditionTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roads_RoadConditionTypes_RoadConditionTypeId",
                        column: x => x.RoadConditionTypeId,
                        principalTable: "RoadConditionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Anomalies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Frame_url = table.Column<string>(maxLength: 100, nullable: false),
                    PotholeConfidence = table.Column<double>(nullable: false),
                    BumpConfidence = table.Column<double>(nullable: false),
                    ManholeConfidence = table.Column<double>(nullable: false),
                    RumbleStripConfidence = table.Column<double>(nullable: false),
                    Lng = table.Column<double>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    TrueCreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 5, 15, 6, 40, 56, 361, DateTimeKind.Local).AddTicks(2632)),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 5, 15, 6, 40, 56, 366, DateTimeKind.Local).AddTicks(6434)),
                    RoadId = table.Column<int>(nullable: false),
                    AnomalyTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anomalies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anomalies_AnomalyTypes_AnomalyTypeId",
                        column: x => x.AnomalyTypeId,
                        principalTable: "AnomalyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Anomalies_Roads_RoadId",
                        column: x => x.RoadId,
                        principalTable: "Roads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AnomalyTypes",
                columns: new[] { "Id", "AnomalyTypeName" },
                values: new object[,]
                {
                    { 1, "Pothole" },
                    { 2, "Bump" },
                    { 3, "Manhole" },
                    { 4, "RumbleStrip" }
                });

            migrationBuilder.InsertData(
                table: "RoadConditionTypes",
                columns: new[] { "Id", "ConditionTypeName" },
                values: new object[,]
                {
                    { 1, "excellent" },
                    { 2, "good" },
                    { 3, "bad" },
                    { 4, "undetermined" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anomalies_AnomalyTypeId",
                table: "Anomalies",
                column: "AnomalyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Anomalies_RoadId",
                table: "Anomalies",
                column: "RoadId");

            migrationBuilder.CreateIndex(
                name: "IX_Roads_RoadConditionTypeId",
                table: "Roads",
                column: "RoadConditionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anomalies");

            migrationBuilder.DropTable(
                name: "AnomalyTypes");

            migrationBuilder.DropTable(
                name: "Roads");

            migrationBuilder.DropTable(
                name: "RoadConditionTypes");
        }
    }
}
