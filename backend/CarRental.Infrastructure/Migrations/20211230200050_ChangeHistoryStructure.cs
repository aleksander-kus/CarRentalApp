using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Infrastructure.Migrations
{
    public partial class ChangeHistoryStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarHistory_Cars_CarID",
                table: "CarHistory");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_CarHistory_CarID",
                table: "CarHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarEmailedEvent",
                table: "CarEmailedEvent");

            migrationBuilder.DropColumn(
                name: "CarID",
                table: "CarHistory");

            migrationBuilder.RenameTable(
                name: "CarEmailedEvent",
                newName: "CarEmailedEvents");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CarHistory",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarBrand",
                table: "CarHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarModel",
                table: "CarHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarProvider",
                table: "CarHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarEmailedEvents",
                table: "CarEmailedEvents",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistory_StartDate_EndDate",
                table: "CarHistory",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CarHistory_UserId",
                table: "CarHistory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CarHistory_StartDate_EndDate",
                table: "CarHistory");

            migrationBuilder.DropIndex(
                name: "IX_CarHistory_UserId",
                table: "CarHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarEmailedEvents",
                table: "CarEmailedEvents");

            migrationBuilder.DropColumn(
                name: "CarBrand",
                table: "CarHistory");

            migrationBuilder.DropColumn(
                name: "CarModel",
                table: "CarHistory");

            migrationBuilder.DropColumn(
                name: "CarProvider",
                table: "CarHistory");

            migrationBuilder.RenameTable(
                name: "CarEmailedEvents",
                newName: "CarEmailedEvent");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CarHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarID",
                table: "CarHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarEmailedEvent",
                table: "CarEmailedEvent",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderCarId = table.Column<int>(type: "int", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarHistory_CarID",
                table: "CarHistory",
                column: "CarID");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistory_Cars_CarID",
                table: "CarHistory",
                column: "CarID",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
