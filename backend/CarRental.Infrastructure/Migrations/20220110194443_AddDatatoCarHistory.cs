using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Infrastructure.Migrations
{
    public partial class AddDatatoCarHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CarHistory_StartDate_EndDate",
                table: "CarHistory");

            migrationBuilder.DropIndex(
                name: "IX_CarHistory_UserId",
                table: "CarHistory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "CarHistory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "CarHistory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsRentConfirmed",
                table: "CarHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PriceId",
                table: "CarHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RentId",
                table: "CarHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Returned",
                table: "CarHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "CarHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarHistory_IsRentConfirmed",
                table: "CarHistory",
                column: "IsRentConfirmed");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistory_Returned_IsRentConfirmed",
                table: "CarHistory",
                columns: new[] { "Returned", "IsRentConfirmed" });

            migrationBuilder.CreateIndex(
                name: "IX_CarHistory_UserId_IsRentConfirmed",
                table: "CarHistory",
                columns: new[] { "UserId", "IsRentConfirmed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CarHistory_IsRentConfirmed",
                table: "CarHistory");

            migrationBuilder.DropIndex(
                name: "IX_CarHistory_Returned_IsRentConfirmed",
                table: "CarHistory");

            migrationBuilder.DropIndex(
                name: "IX_CarHistory_UserId_IsRentConfirmed",
                table: "CarHistory");

            migrationBuilder.DropColumn(
                name: "IsRentConfirmed",
                table: "CarHistory");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "CarHistory");

            migrationBuilder.DropColumn(
                name: "RentId",
                table: "CarHistory");

            migrationBuilder.DropColumn(
                name: "Returned",
                table: "CarHistory");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "CarHistory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "CarHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "CarHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarHistory_StartDate_EndDate",
                table: "CarHistory",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CarHistory_UserId",
                table: "CarHistory",
                column: "UserId");
        }
    }
}
