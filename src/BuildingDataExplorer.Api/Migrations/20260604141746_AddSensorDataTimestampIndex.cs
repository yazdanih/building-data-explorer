using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingDataExplorer.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSensorDataTimestampIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SensorData_RoomId",
                table: "SensorData");

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_RoomId_Timestamp",
                table: "SensorData",
                columns: new[] { "RoomId", "Timestamp" },
                descending: new[] { false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SensorData_RoomId_Timestamp",
                table: "SensorData");

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_RoomId",
                table: "SensorData",
                column: "RoomId");
        }
    }
}
