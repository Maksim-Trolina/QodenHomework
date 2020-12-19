using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountName = table.Column<string>(type: "text", nullable: false),
                    UserMail = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountName);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyName = table.Column<string>(type: "text", nullable: true),
                    Count = table.Column<decimal>(type: "numeric", nullable: false),
                    AccountName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyAlls",
                columns: table => new
                {
                    CurrencyName = table.Column<string>(type: "text", nullable: false),
                    InputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    OutputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    InputLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    OutputLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferLimit = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyAlls", x => x.CurrencyName);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyName = table.Column<string>(type: "text", nullable: true),
                    Mail = table.Column<string>(type: "text", nullable: true),
                    InputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    OutputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferCommision = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "CurrencyAccounts");

            migrationBuilder.DropTable(
                name: "CurrencyAlls");

            migrationBuilder.DropTable(
                name: "CurrencyUsers");
        }
    }
}
