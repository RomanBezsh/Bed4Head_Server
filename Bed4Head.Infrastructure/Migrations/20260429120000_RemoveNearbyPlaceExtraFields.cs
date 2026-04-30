using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bed4Head.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNearbyPlaceExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "NearbyPlaces");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "NearbyPlaces");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "NearbyPlaces");

            migrationBuilder.DropColumn(
                name: "WalkingMinutes",
                table: "NearbyPlaces");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "NearbyPlaces",
                type: "text",
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "WalkingMinutes",
                table: "NearbyPlaces",
                type: "integer",
                nullable: true);
        }
    }
}
