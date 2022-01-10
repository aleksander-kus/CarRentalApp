using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Infrastructure.Migrations
{
    public partial class AddReturnsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarReturnEntries",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HistoryEntryId = table.Column<int>(type: "int", nullable: false),
                    PhotoFileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OdometerValue = table.Column<double>(type: "float", nullable: false),
                    CarCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarReturnEntries", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarReturnEntries");
        }
    }
}
