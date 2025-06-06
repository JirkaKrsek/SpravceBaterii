using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpravceBaterii.Migrations
{
    /// <inheritdoc />
    public partial class AddBatteryTypeConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Batteries ADD CONSTRAINT Check_Battery_OnlyOneType " +
            "CHECK ((IsRechargeable = 0 AND DisposableBatteryId IS NOT NULL AND RechargeableBatteryId IS NULL) OR " +
            "(IsRechargeable = 1 AND DisposableBatteryId IS NULL AND RechargeableBatteryId IS NOT NULL))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Batteries DROP CONSTRAINT Check_Battery_OnlyOneType");
        }
    }
}
