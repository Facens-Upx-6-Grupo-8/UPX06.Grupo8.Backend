using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSampleEvaluator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SampleSetPointSettings");

            migrationBuilder.CreateTable(
                name: "SampleEvaluator",
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
                    table.PrimaryKey("PK_SampleEvaluator", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SampleEvaluator");

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
    }
}
