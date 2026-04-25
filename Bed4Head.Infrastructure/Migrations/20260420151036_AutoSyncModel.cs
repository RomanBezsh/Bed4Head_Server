using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bed4Head.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AutoSyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AreaInSquareMeters",
                table: "Rooms",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvailableUnits",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "BreakfastIncluded",
                table: "Rooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Rooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PrivateBathroom",
                table: "Rooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RoomType",
                table: "Rooms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "View",
                table: "Rooms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerifiedStay",
                table: "Reviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "OverallScore",
                table: "Reviews",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StayedAt",
                table: "Reviews",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reviews",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TripType",
                table: "Reviews",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "NearbyPlaces",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalkingMinutes",
                table: "NearbyPlaces",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePricePerNight",
                table: "Hotels",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckInFrom",
                table: "Hotels",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckOutUntil",
                table: "Hotels",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Hotels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Hotels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DistanceFromCenterKm",
                table: "Hotels",
                type: "double precision",
                nullable: true);

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

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Hotels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PetsAllowed",
                table: "Hotels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Hotels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RatingLabel",
                table: "Hotels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Hotels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Hotels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "HotelPhotos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "HotelPhotos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Amenities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsHighlighted",
                table: "Amenities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaInSquareMeters",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AvailableUnits",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BreakfastIncluded",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PrivateBathroom",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "View",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IsVerifiedStay",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "OverallScore",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "StayedAt",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "TripType",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "NearbyPlaces");

            migrationBuilder.DropColumn(
                name: "WalkingMinutes",
                table: "NearbyPlaces");

            migrationBuilder.DropColumn(
                name: "BasePricePerNight",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CheckInFrom",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CheckOutUntil",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "DistanceFromCenterKm",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "HasFreeWifi",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "HasParking",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "PetsAllowed",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "RatingLabel",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "HotelPhotos");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "HotelPhotos");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "IsHighlighted",
                table: "Amenities");
        }
    }
}
