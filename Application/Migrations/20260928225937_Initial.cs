using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SourcingPoint = table.Column<int>(type: "INTEGER", nullable: false),
                    PH = table.Column<double>(type: "REAL", nullable: false),
                    Turbidity = table.Column<double>(type: "REAL", nullable: false),
                    Temperature = table.Column<double>(type: "REAL", nullable: false),
                    TDS = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.Id);
                    table.UniqueConstraint("AK_Samples_Timestamp_SourcingPoint", x => new { x.Timestamp, x.SourcingPoint });
                });

            migrationBuilder.CreateTable(
                name: "SampleSetPointSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PHSettings_IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    PHSettings_LowerBound = table.Column<double>(type: "REAL", nullable: false),
                    PHSettings_UpperBound = table.Column<double>(type: "REAL", nullable: false),
                    TDSSettings_IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    TDSSettings_LowerBound = table.Column<double>(type: "REAL", nullable: false),
                    TDSSettings_UpperBound = table.Column<double>(type: "REAL", nullable: false),
                    TemperatureSettings_IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    TemperatureSettings_LowerBound = table.Column<double>(type: "REAL", nullable: false),
                    TemperatureSettings_UpperBound = table.Column<double>(type: "REAL", nullable: false),
                    TurbiditySettings_IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    TurbiditySettings_LowerBound = table.Column<double>(type: "REAL", nullable: false),
                    TurbiditySettings_UpperBound = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleSetPointSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "SampleSetPointSettings");
        }
    }
}
