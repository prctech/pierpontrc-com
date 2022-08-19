using Microsoft.EntityFrameworkCore.Migrations;

namespace PRC.Data.Migrations
{
    public partial class MenuIdsOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Page",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Menu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Page",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
