using Bed4Head.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bed4Head.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260425124300_AddPreferredCurrencyCodeToUser")]
    public partial class AddPreferredCurrencyCodeToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferredCurrencyCode",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredCurrencyCode",
                table: "Users");
        }
    }
}

