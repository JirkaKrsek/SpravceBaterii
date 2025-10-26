using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpravceBaterii.Migrations
{
    /// <inheritdoc />
    public partial class AddBatteryTypeToDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatteryTypeId",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_BatteryTypeId",
                table: "Devices",
                column: "BatteryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_BatteryTypes_BatteryTypeId",
                table: "Devices",
                column: "BatteryTypeId",
                principalTable: "BatteryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_BatteryTypes_BatteryTypeId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_BatteryTypeId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "BatteryTypeId",
                table: "Devices");
        }
    }
}
