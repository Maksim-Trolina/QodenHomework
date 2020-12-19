using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUniqueCommision",
                table: "CurrencyUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountName", "Password", "Role", "UserMail" },
                values: new object[] { "Admin", "Admin", (byte)0, "Admin" });

            migrationBuilder.InsertData(
                table: "CurrencyAlls",
                columns: new[] { "CurrencyName", "InputCommision", "InputLimit", "OutputCommision", "OutputLimit", "TransferCommision", "TransferLimit" },
                values: new object[] { "USD", 0m, 0m, 0m, 0m, 0m, 0m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountName",
                keyValue: "Admin");

            migrationBuilder.DeleteData(
                table: "CurrencyAlls",
                keyColumn: "CurrencyName",
                keyValue: "USD");

            migrationBuilder.DropColumn(
                name: "IsUniqueCommision",
                table: "CurrencyUsers");
        }
    }
}
