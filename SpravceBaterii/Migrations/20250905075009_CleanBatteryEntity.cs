using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpravceBaterii.Migrations
{
    /// <inheritdoc />
    public partial class CleanBatteryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisposableBatteryId",
                table: "Batteries");

            migrationBuilder.DropColumn(
                name: "RechargeableBatteryId",
                table: "Batteries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisposableBatteryId",
                table: "Batteries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RechargeableBatteryId",
                table: "Batteries",
                type: "int",
                nullable: true);
        }
    }
}
