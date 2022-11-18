using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThAmCo.Events.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientReferenceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Guest",
                columns: table => new
                {
                    GuestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Forename = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Telephone = table.Column<int>(type: "INTEGER", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.GuestId);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Forename = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    JobRole = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffId);
                });

            migrationBuilder.CreateTable(
                name: "GuestBooking",
                columns: table => new
                {
                    GuestId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestBooking", x => new { x.EventId, x.GuestId });
                    table.ForeignKey(
                        name: "FK_GuestBooking_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuestBooking_Guest_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guest",
                        principalColumn: "GuestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staffing",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffing", x => new { x.StaffId, x.EventId });
                    table.ForeignKey(
                        name: "FK_Staffing_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Staffing_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Event",
                columns: new[] { "EventId", "ClientReferenceId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Event",
                columns: new[] { "EventId", "ClientReferenceId" },
                values: new object[] { 2, 2 });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "GuestId", "Address", "Email", "Forename", "Surname", "Telephone" },
                values: new object[] { 1, "21 jump street", "RichardNorth@email.com", "Richard", "North", 164266223 });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "GuestId", "Address", "Email", "Forename", "Surname", "Telephone" },
                values: new object[] { 2, "22 hop street", "DanielNorth@email.com", "Daniel", "North", 164263223 });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "GuestId", "Address", "Email", "Forename", "Surname", "Telephone" },
                values: new object[] { 3, "23 skip street", "MichealNorth@email.com", "Micheal", "North", 1642642223 });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "GuestId", "Address", "Email", "Forename", "Surname", "Telephone" },
                values: new object[] { 4, "24 run street", "RobertNorth@email.com", "Robert", "North", 163366223 });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "GuestId", "Address", "Email", "Forename", "Surname", "Telephone" },
                values: new object[] { 5, "25 skid street", "AidanNorth@email.com", "Aidan", "North", 164266213 });

            migrationBuilder.InsertData(
                table: "Guest",
                columns: new[] { "GuestId", "Address", "Email", "Forename", "Surname", "Telephone" },
                values: new object[] { 6, "26 slide street", "ThomasNorth@email.com", "Thomas", "North", 164266923 });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "Email", "Forename", "JobRole", "Password", "Surname" },
                values: new object[] { 1, "chelseacopeland@email.com", "Chelsea", 0, "password", "Copland" });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "Email", "Forename", "JobRole", "Password", "Surname" },
                values: new object[] { 2, "michellecopeland@email.com", "michelle", 2, "password", "Copland" });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "Email", "Forename", "JobRole", "Password", "Surname" },
                values: new object[] { 3, "Carlycopeland@email.com", "Carly", 2, "password", "Copland" });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "Email", "Forename", "JobRole", "Password", "Surname" },
                values: new object[] { 4, "ciaracopeland@email.com", "ciara", 2, "password", "Copland" });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "Email", "Forename", "JobRole", "Password", "Surname" },
                values: new object[] { 5, "annacopeland@email.com", "anna", 2, "password", "Copland" });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "Email", "Forename", "JobRole", "Password", "Surname" },
                values: new object[] { 6, "deecopeland@email.com", "dee", 1, "password", "Copland" });

            migrationBuilder.InsertData(
                table: "GuestBooking",
                columns: new[] { "EventId", "GuestId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "GuestBooking",
                columns: new[] { "EventId", "GuestId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "GuestBooking",
                columns: new[] { "EventId", "GuestId" },
                values: new object[] { 1, 3 });

            migrationBuilder.InsertData(
                table: "GuestBooking",
                columns: new[] { "EventId", "GuestId" },
                values: new object[] { 1, 4 });

            migrationBuilder.InsertData(
                table: "GuestBooking",
                columns: new[] { "EventId", "GuestId" },
                values: new object[] { 1, 5 });

            migrationBuilder.InsertData(
                table: "GuestBooking",
                columns: new[] { "EventId", "GuestId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "GuestBooking",
                columns: new[] { "EventId", "GuestId" },
                values: new object[] { 2, 4 });

            migrationBuilder.InsertData(
                table: "GuestBooking",
                columns: new[] { "EventId", "GuestId" },
                values: new object[] { 2, 6 });

            migrationBuilder.InsertData(
                table: "Staffing",
                columns: new[] { "EventId", "StaffId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Staffing",
                columns: new[] { "EventId", "StaffId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "Staffing",
                columns: new[] { "EventId", "StaffId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "Staffing",
                columns: new[] { "EventId", "StaffId" },
                values: new object[] { 1, 3 });

            migrationBuilder.InsertData(
                table: "Staffing",
                columns: new[] { "EventId", "StaffId" },
                values: new object[] { 2, 3 });

            migrationBuilder.InsertData(
                table: "Staffing",
                columns: new[] { "EventId", "StaffId" },
                values: new object[] { 1, 4 });

            migrationBuilder.InsertData(
                table: "Staffing",
                columns: new[] { "EventId", "StaffId" },
                values: new object[] { 2, 6 });

            migrationBuilder.CreateIndex(
                name: "IX_GuestBooking_GuestId",
                table: "GuestBooking",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffing_EventId",
                table: "Staffing",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuestBooking");

            migrationBuilder.DropTable(
                name: "Staffing");

            migrationBuilder.DropTable(
                name: "Guest");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Staff");
        }
    }
}
