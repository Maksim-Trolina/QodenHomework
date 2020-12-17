using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    UserMail = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.UserMail);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyNames",
                columns: table => new
                {
                    CurrencyName = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_CurrencyNames", x => x.CurrencyName);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyUsers",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "text", nullable: false),
                    CurrencyName = table.Column<string>(type: "text", nullable: true),
                    Count = table.Column<decimal>(type: "numeric", nullable: false),
                    InputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    OutputCommision = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferCommision = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyUsers", x => x.UserName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "CurrencyNames");

            migrationBuilder.DropTable(
                name: "CurrencyUsers");
        }
    }
}
