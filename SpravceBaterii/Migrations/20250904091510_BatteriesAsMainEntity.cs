using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpravceBaterii.Migrations
{
    /// <inheritdoc />
    public partial class BatteriesAsMainEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batteries_Devices_DeviceId",
                table: "Batteries");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Locations_LocationId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_AspNetUsers_UserId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_UserId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "LastLogin",
                table: "AspNetUsers",
                newName: "LastLoginDate");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Devices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceId",
                table: "Batteries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Batteries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Batteries_UserId",
                table: "Batteries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batteries_AspNetUsers_UserId",
                table: "Batteries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Batteries_Devices_DeviceId",
                table: "Batteries",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Locations_LocationId",
                table: "Devices",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batteries_AspNetUsers_UserId",
                table: "Batteries");

            migrationBuilder.DropForeignKey(
                name: "FK_Batteries_Devices_DeviceId",
                table: "Batteries");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Locations_LocationId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Batteries_UserId",
                table: "Batteries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Batteries");

            migrationBuilder.RenameColumn(
                name: "LastLoginDate",
                table: "AspNetUsers",
                newName: "LastLogin");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeviceId",
                table: "Batteries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_UserId",
                table: "Locations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batteries_Devices_DeviceId",
                table: "Batteries",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Locations_LocationId",
                table: "Devices",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_AspNetUsers_UserId",
                table: "Locations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
