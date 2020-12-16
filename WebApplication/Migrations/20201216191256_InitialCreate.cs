using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f49d9e6e-f676-46fc-866f-a8d2abf45518"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "MyParam", "Password", "Role", "UserName" },
                values: new object[] { new Guid("4dbecf79-c580-4779-84d7-5ede59e29a62"), null, "Admin", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4dbecf79-c580-4779-84d7-5ede59e29a62"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "MyParam", "Password", "Role", "UserName" },
                values: new object[] { new Guid("f49d9e6e-f676-46fc-866f-a8d2abf45518"), null, "Admin", "Admin", "Admin" });
        }
    }
}
