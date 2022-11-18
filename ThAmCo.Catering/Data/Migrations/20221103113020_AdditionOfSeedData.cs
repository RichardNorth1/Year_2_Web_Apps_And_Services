using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThAmCo.Catering.Data.Migrations
{
    public partial class AdditionOfSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FoodItem",
                columns: new[] { "FoodItemId", "Description", "UnitPrice" },
                values: new object[] { 1, "fish", 6.0 });

            migrationBuilder.InsertData(
                table: "FoodItem",
                columns: new[] { "FoodItemId", "Description", "UnitPrice" },
                values: new object[] { 2, "chips", 2.5 });

            migrationBuilder.InsertData(
                table: "FoodItem",
                columns: new[] { "FoodItemId", "Description", "UnitPrice" },
                values: new object[] { 3, "peas", 1.0 });

            migrationBuilder.InsertData(
                table: "FoodItem",
                columns: new[] { "FoodItemId", "Description", "UnitPrice" },
                values: new object[] { 4, "carrots", 1.0 });

            migrationBuilder.InsertData(
                table: "FoodItem",
                columns: new[] { "FoodItemId", "Description", "UnitPrice" },
                values: new object[] { 5, "steak", 9.0 });

            migrationBuilder.InsertData(
                table: "FoodItem",
                columns: new[] { "FoodItemId", "Description", "UnitPrice" },
                values: new object[] { 6, "lobster", 10.0 });

            migrationBuilder.InsertData(
                table: "FoodItem",
                columns: new[] { "FoodItemId", "Description", "UnitPrice" },
                values: new object[] { 7, "gammon", 5.0 });

            migrationBuilder.InsertData(
                table: "Menu",
                columns: new[] { "MenuId", "MenuName" },
                values: new object[] { 1, "Menu 1" });

            migrationBuilder.InsertData(
                table: "Menu",
                columns: new[] { "MenuId", "MenuName" },
                values: new object[] { 2, "Menu 2" });

            migrationBuilder.InsertData(
                table: "FoodBooking",
                columns: new[] { "FoodBookingId", "ClientReferenceId", "MenuId", "NumberOfGuests" },
                values: new object[] { 1, 1, 1, 50 });

            migrationBuilder.InsertData(
                table: "FoodBooking",
                columns: new[] { "FoodBookingId", "ClientReferenceId", "MenuId", "NumberOfGuests" },
                values: new object[] { 2, 2, 2, 50 });

            migrationBuilder.InsertData(
                table: "MenuFoodItem",
                columns: new[] { "FoodItemId", "MenuId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "MenuFoodItem",
                columns: new[] { "FoodItemId", "MenuId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "MenuFoodItem",
                columns: new[] { "FoodItemId", "MenuId" },
                values: new object[] { 3, 1 });

            migrationBuilder.InsertData(
                table: "MenuFoodItem",
                columns: new[] { "FoodItemId", "MenuId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "MenuFoodItem",
                columns: new[] { "FoodItemId", "MenuId" },
                values: new object[] { 4, 2 });

            migrationBuilder.InsertData(
                table: "MenuFoodItem",
                columns: new[] { "FoodItemId", "MenuId" },
                values: new object[] { 5, 2 });

            migrationBuilder.InsertData(
                table: "MenuFoodItem",
                columns: new[] { "FoodItemId", "MenuId" },
                values: new object[] { 6, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FoodBooking",
                keyColumn: "FoodBookingId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FoodBooking",
                keyColumn: "FoodBookingId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FoodItem",
                keyColumn: "FoodItemId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MenuFoodItem",
                keyColumns: new[] { "FoodItemId", "MenuId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "MenuFoodItem",
                keyColumns: new[] { "FoodItemId", "MenuId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "MenuFoodItem",
                keyColumns: new[] { "FoodItemId", "MenuId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "MenuFoodItem",
                keyColumns: new[] { "FoodItemId", "MenuId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "MenuFoodItem",
                keyColumns: new[] { "FoodItemId", "MenuId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "MenuFoodItem",
                keyColumns: new[] { "FoodItemId", "MenuId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "MenuFoodItem",
                keyColumns: new[] { "FoodItemId", "MenuId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "FoodItem",
                keyColumn: "FoodItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FoodItem",
                keyColumn: "FoodItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FoodItem",
                keyColumn: "FoodItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FoodItem",
                keyColumn: "FoodItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FoodItem",
                keyColumn: "FoodItemId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FoodItem",
                keyColumn: "FoodItemId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Menu",
                keyColumn: "MenuId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Menu",
                keyColumn: "MenuId",
                keyValue: 2);
        }
    }
}
