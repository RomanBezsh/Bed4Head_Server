using Bed4Head.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bed4Head.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260425123400_RemoveHotelBooleanAmenities")]
    public partial class RemoveHotelBooleanAmenities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PetsAllowed",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "HasFreeWifi",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "HasParking",
                table: "Hotels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PetsAllowed",
                table: "Hotels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasFreeWifi",
                table: "Hotels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasParking",
                table: "Hotels",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

