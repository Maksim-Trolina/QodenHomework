using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("37dca5a0-83b6-4ace-bd89-e141670dd2d3"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("91ed60a4-8a9d-404d-ad91-f067c1f2b8a5"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "RegistrationDate", "Role" },
                values: new object[] { new Guid("efa61c35-cb88-46d4-856b-c4b7c45360e8"), "Admin@com", new DateTime(2020, 12, 30, 4, 7, 16, 928, DateTimeKind.Local).AddTicks(8019), 0 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "Password", "RegistrationDate", "UserId" },
                values: new object[] { new Guid("06c7fc1f-2e4c-4f68-bd40-8110e79726c3"), "Admin", "Admin", new DateTime(2020, 12, 30, 4, 7, 16, 933, DateTimeKind.Local).AddTicks(3101), new Guid("efa61c35-cb88-46d4-856b-c4b7c45360e8") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("91ed60a4-8a9d-404d-ad91-f067c1f2b8a5"), "Admin@com", new DateTime(2020, 12, 29, 8, 44, 39, 306, DateTimeKind.Local).AddTicks(7984), 0 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "Password", "RegistrationDate", "UserId" },
                values: new object[] { new Guid("37dca5a0-83b6-4ace-bd89-e141670dd2d3"), "Admin", "Admin", new DateTime(2020, 12, 29, 8, 44, 39, 311, DateTimeKind.Local).AddTicks(2214), new Guid("91ed60a4-8a9d-404d-ad91-f067c1f2b8a5") });
        }
    }
}
