using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bed4Head.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNearbyPlaceCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "NearbyPlaces");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "NearbyPlaces");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "NearbyPlaces",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "NearbyPlaces",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
