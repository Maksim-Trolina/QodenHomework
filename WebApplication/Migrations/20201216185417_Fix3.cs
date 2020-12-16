using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class Fix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2423da1f-d56c-4945-af94-593e2d8d56b4"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "UserName" },
                values: new object[] { new Guid("e6c95072-56e2-426a-b2be-a3e3e843aae0"), "Admin", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e6c95072-56e2-426a-b2be-a3e3e843aae0"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "UserName" },
                values: new object[] { new Guid("2423da1f-d56c-4945-af94-593e2d8d56b4"), "Admin", "Admin", "Admin" });
        }
    }
}
