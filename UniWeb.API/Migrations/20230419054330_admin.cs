using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carewell.API.Migrations
{
    public partial class admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "admin@uniweb.com");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "uniweb@admin");
        }
    }
}
