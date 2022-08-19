using Microsoft.EntityFrameworkCore.Migrations;

namespace PRC.Data.Migrations
{
    public partial class MenuName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Menu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Menu");
        }
    }
}
