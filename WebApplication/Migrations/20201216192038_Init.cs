using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4dbecf79-c580-4779-84d7-5ede59e29a62"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "MyParam", "Password", "Role", "UserName" },
                values: new object[] { new Guid("7d6f7ff9-9744-4626-8f7e-b54d5ea504bf"), null, "Admin", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7d6f7ff9-9744-4626-8f7e-b54d5ea504bf"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "MyParam", "Password", "Role", "UserName" },
                values: new object[] { new Guid("4dbecf79-c580-4779-84d7-5ede59e29a62"), null, "Admin", "Admin", "Admin" });
        }
    }
}
