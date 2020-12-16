using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class MyFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e6c95072-56e2-426a-b2be-a3e3e843aae0"));

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "MyParam",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "MyParam", "Password", "Role", "UserName" },
                values: new object[] { new Guid("f49d9e6e-f676-46fc-866f-a8d2abf45518"), null, "Admin", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f49d9e6e-f676-46fc-866f-a8d2abf45518"));

            migrationBuilder.DropColumn(
                name: "MyParam",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "TestId",
                table: "Accounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "UserName" },
                values: new object[] { new Guid("e6c95072-56e2-426a-b2be-a3e3e843aae0"), "Admin", "Admin", "Admin" });
        }
    }
}
