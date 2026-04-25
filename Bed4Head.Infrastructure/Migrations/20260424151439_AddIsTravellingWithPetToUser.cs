using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bed4Head.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTravellingWithPetToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTravellingWithPet",
                table: "Users",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTravellingWithPet",
                table: "Users");
        }
    }
}
