using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("7daf5156-f725-428a-bcb1-b5e645c2a214"), "Admin@com", new DateTime(2020, 12, 31, 3, 30, 33, 311, DateTimeKind.Local).AddTicks(5581), 0 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "Password", "RegistrationDate", "UserId" },
                values: new object[] { new Guid("749d4cda-85c8-4af7-8fd5-1bfadeefed86"), "Admin", "Admin", new DateTime(2020, 12, 31, 3, 30, 33, 316, DateTimeKind.Local).AddTicks(1814), new Guid("7daf5156-f725-428a-bcb1-b5e645c2a214") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("749d4cda-85c8-4af7-8fd5-1bfadeefed86"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7daf5156-f725-428a-bcb1-b5e645c2a214"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "RegistrationDate", "Role" },
                values: new object[] { new Guid("c734429d-2461-4db5-b923-8aa3ff934183"), "Admin@com", new DateTime(2020, 12, 30, 22, 0, 37, 736, DateTimeKind.Local).AddTicks(3761), 0 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "Password", "RegistrationDate", "UserId" },
                values: new object[] { new Guid("a3946cd2-bf8a-4746-909e-b3dbc8367a6b"), "Admin", "Admin", new DateTime(2020, 12, 30, 22, 0, 37, 741, DateTimeKind.Local).AddTicks(1159), new Guid("c734429d-2461-4db5-b923-8aa3ff934183") });
        }
    }
}
