using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpravceBaterii.Migrations
{
    /// <inheritdoc />
    public partial class RemoveChargingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChargingHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChargingHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RechargeableBatteryId = table.Column<int>(type: "int", nullable: false),
                    CapacityAfterCharge = table.Column<int>(type: "int", nullable: true),
                    ChargeDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargingHistories_RechargeableBatteries_RechargeableBatteryId",
                        column: x => x.RechargeableBatteryId,
                        principalTable: "RechargeableBatteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargingHistories_RechargeableBatteryId",
                table: "ChargingHistories",
                column: "RechargeableBatteryId");
        }
    }
}
