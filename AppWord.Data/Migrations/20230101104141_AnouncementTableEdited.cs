using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWord.Data.Migrations
{
    public partial class AnouncementTableEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Announcements");

            migrationBuilder.AddColumn<int>(
                name: "AnnouncementType",
                table: "Announcements",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnouncementType",
                table: "Announcements");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Announcements",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
