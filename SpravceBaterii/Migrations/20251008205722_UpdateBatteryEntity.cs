using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpravceBaterii.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBatteryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "InsertionDate",
                table: "Batteries",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Batteries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Batteries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsageHistory",
                table: "Batteries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Batteries");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Batteries");

            migrationBuilder.DropColumn(
                name: "UsageHistory",
                table: "Batteries");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "InsertionDate",
                table: "Batteries",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }
    }
}
