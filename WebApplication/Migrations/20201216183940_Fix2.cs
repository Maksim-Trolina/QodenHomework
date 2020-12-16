using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class Fix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e8a35336-e884-4931-96ca-804f6fb923b5"));

            migrationBuilder.AddColumn<Guid>(
                name: "TestId",
                table: "Accounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "UserName" },
                values: new object[] { new Guid("2423da1f-d56c-4945-af94-593e2d8d56b4"), "Admin", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2423da1f-d56c-4945-af94-593e2d8d56b4"));

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Accounts");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "UserName" },
                values: new object[] { new Guid("e8a35336-e884-4931-96ca-804f6fb923b5"), "Admin", "Admin", "Admin" });
        }
    }
}
