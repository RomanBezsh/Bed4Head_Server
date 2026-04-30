using Bed4Head.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bed4Head.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260430220500_AddBookingContactPreferences")]
    public partial class AddBookingContactPreferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CallMe",
                table: "Bookings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SendEmail",
                table: "Bookings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallMe",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "SendEmail",
                table: "Bookings");
        }
    }
}
