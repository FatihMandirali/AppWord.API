using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWord.Data.Migrations
{
    public partial class WordTableEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WordOfDayCount",
                table: "Words",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "WordOfDayDate",
                table: "Words",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordOfDayCount",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "WordOfDayDate",
                table: "Words");
        }
    }
}
