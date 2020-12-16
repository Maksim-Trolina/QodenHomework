using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyNames",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Coast = table.Column<decimal>(type: "numeric", nullable: false),
                    InputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    OutputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    InputLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    OutputLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferLimit = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    InputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    OutputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    InputLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    OutputLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferLimit = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CurrencyNames",
                columns: new[] { "Id", "Coast", "InputCommision", "InputLimit", "OutputCommision", "OutputLimit", "TransferCommision", "TransferLimit" },
                values: new object[] { "bucks", 0m, 0m, 0m, 0m, 0m, 0m, 0m });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "UserName" },
                values: new object[] { new Guid("e8a35336-e884-4931-96ca-804f6fb923b5"), "Admin", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "CurrencyNames");

            migrationBuilder.DropTable(
                name: "CurrencyUsers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
