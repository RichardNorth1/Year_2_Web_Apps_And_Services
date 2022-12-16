using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThAmCo.Events.Data.Migrations
{
    public partial class AdditionOfFurtherEventFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MenuForEvent",
                table: "Event",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VenueName",
                table: "Event",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuForEvent",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "VenueName",
                table: "Event");
        }
    }
}
