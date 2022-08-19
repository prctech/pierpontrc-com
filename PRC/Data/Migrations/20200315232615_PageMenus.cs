using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PRC.Data.Migrations
{
    public partial class PageMenus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Contact",
                table: "Pages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "Pages",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ParentPage",
                table: "Pages",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "SubPages",
                table: "Pages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ParentPage",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "SubPages",
                table: "Pages");
        }
    }
}
