using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("06c7fc1f-2e4c-4f68-bd40-8110e79726c3"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("efa61c35-cb88-46d4-856b-c4b7c45360e8"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "RegistrationDate", "Role" },
                values: new object[] { new Guid("c734429d-2461-4db5-b923-8aa3ff934183"), "Admin@com", new DateTime(2020, 12, 30, 22, 0, 37, 736, DateTimeKind.Local).AddTicks(3761), 0 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "Password", "RegistrationDate", "UserId" },
                values: new object[] { new Guid("a3946cd2-bf8a-4746-909e-b3dbc8367a6b"), "Admin", "Admin", new DateTime(2020, 12, 30, 22, 0, 37, 741, DateTimeKind.Local).AddTicks(1159), new Guid("c734429d-2461-4db5-b923-8aa3ff934183") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("a3946cd2-bf8a-4746-909e-b3dbc8367a6b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c734429d-2461-4db5-b923-8aa3ff934183"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "RegistrationDate", "Role" },
                values: new object[] { new Guid("efa61c35-cb88-46d4-856b-c4b7c45360e8"), "Admin@com", new DateTime(2020, 12, 30, 4, 7, 16, 928, DateTimeKind.Local).AddTicks(8019), 0 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "Password", "RegistrationDate", "UserId" },
                values: new object[] { new Guid("06c7fc1f-2e4c-4f68-bd40-8110e79726c3"), "Admin", "Admin", new DateTime(2020, 12, 30, 4, 7, 16, 933, DateTimeKind.Local).AddTicks(3101), new Guid("efa61c35-cb88-46d4-856b-c4b7c45360e8") });
        }
    }
}
