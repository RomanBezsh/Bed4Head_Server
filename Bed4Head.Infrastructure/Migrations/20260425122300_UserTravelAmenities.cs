using System;
using Bed4Head.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bed4Head.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260425122300_UserTravelAmenities")]
    public partial class UserTravelAmenities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTravellingWithPet",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Amenities_AmenityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AmenityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AmenityId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "AmenityUser",
                columns: table => new
                {
                    AmenitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenityUser", x => new { x.AmenitiesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AmenityUser_Amenities_AmenitiesId",
                        column: x => x.AmenitiesId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenityUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmenityUser_UsersId",
                table: "AmenityUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmenityUser");

            migrationBuilder.AddColumn<Guid>(
                name: "AmenityId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTravellingWithPet",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AmenityId",
                table: "Users",
                column: "AmenityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Amenities_AmenityId",
                table: "Users",
                column: "AmenityId",
                principalTable: "Amenities",
                principalColumn: "Id");
        }
    }
}

