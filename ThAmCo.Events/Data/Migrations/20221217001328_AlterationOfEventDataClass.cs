using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThAmCo.Events.Data.Migrations
{
    public partial class AlterationOfEventDataClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventTypeId",
                table: "Event");

            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "Event",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Event");

            migrationBuilder.AddColumn<string>(
                name: "EventTypeId",
                table: "Event",
                type: "TEXT",
                maxLength: 3,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "EventId",
                keyValue: 1,
                column: "EventTypeId",
                value: "PTY");

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "EventId",
                keyValue: 2,
                column: "EventTypeId",
                value: "MET");
        }
    }
}
