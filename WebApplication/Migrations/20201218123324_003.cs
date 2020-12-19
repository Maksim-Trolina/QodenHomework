using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinInput",
                table: "CurrencyAlls",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinOutput",
                table: "CurrencyAlls",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinTransfer",
                table: "CurrencyAlls",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "CurrencyAlls",
                keyColumn: "CurrencyName",
                keyValue: "USD",
                column: "InputCommision",
                value: 0.2m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinInput",
                table: "CurrencyAlls");

            migrationBuilder.DropColumn(
                name: "MinOutput",
                table: "CurrencyAlls");

            migrationBuilder.DropColumn(
                name: "MinTransfer",
                table: "CurrencyAlls");

            migrationBuilder.UpdateData(
                table: "CurrencyAlls",
                keyColumn: "CurrencyName",
                keyValue: "USD",
                column: "InputCommision",
                value: 0m);
        }
    }
}
