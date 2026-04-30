using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bed4Head.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomAmenities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaInSquareMeters",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AvailableUnits",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BedType",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "View",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CheckInFrom",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CheckOutUntil",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "BreakfastIncluded",
                table: "Rooms",
                newName: "HasWifi");

            migrationBuilder.AddColumn<bool>(
                name: "HasPrivatePool",
                table: "Rooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RoomBeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomBeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomBeds_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomBeds_RoomId",
                table: "RoomBeds",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomBeds");

            migrationBuilder.DropColumn(
                name: "HasPrivatePool",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "HasWifi",
                table: "Rooms",
                newName: "BreakfastIncluded");

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

            migrationBuilder.AddColumn<string>(
                name: "BedType",
                table: "Rooms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                type: "text",
                nullable: true);

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
                name: "ShortDescription",
                table: "Hotels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Hotels",
                type: "text",
                nullable: true);
        }
    }
}
