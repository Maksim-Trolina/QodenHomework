using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class IdToGuid2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("92436d8d-4eb7-42d2-9808-1ce514cf9a84"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "password", "role", "user_name" },
                values: new object[] { new Guid("54c2121b-84b4-4b95-9f06-bf82698374c6"), "Admin", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("54c2121b-84b4-4b95-9f06-bf82698374c6"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "password", "role", "user_name" },
                values: new object[] { new Guid("92436d8d-4eb7-42d2-9808-1ce514cf9a84"), "Admin", "Admin", "Admin" });
        }
    }
}
