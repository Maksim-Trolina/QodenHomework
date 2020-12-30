using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyInformation",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    DepositCommission = table.Column<decimal>(type: "numeric", nullable: false),
                    WithdrawCommission = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferCommission = table.Column<decimal>(type: "numeric", nullable: false),
                    DepositLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    WithdrawLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    TransferLimit = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyInformation", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCommissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyName = table.Column<string>(type: "text", nullable: true),
                    DepositCommission = table.Column<decimal>(type: "numeric", nullable: true),
                    WithdrawCommission = table.Column<decimal>(type: "numeric", nullable: true),
                    TransferCommission = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCommissions_CurrencyInformation_CurrencyName",
                        column: x => x.CurrencyName,
                        principalTable: "CurrencyInformation",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCommissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountCurrencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyName = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountCurrencies_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountCurrencies_CurrencyInformation_CurrencyName",
                        column: x => x.CurrencyName,
                        principalTable: "CurrencyInformation",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ToAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CurrencyName = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Accounts_FromAccountId",
                        column: x => x.FromAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_Accounts_ToAccountId",
                        column: x => x.ToAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_CurrencyInformation_CurrencyName",
                        column: x => x.CurrencyName,
                        principalTable: "CurrencyInformation",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "RegistrationDate", "Role" },
                values: new object[] { new Guid("91ed60a4-8a9d-404d-ad91-f067c1f2b8a5"), "Admin@com", new DateTime(2020, 12, 29, 8, 44, 39, 306, DateTimeKind.Local).AddTicks(7984), 0 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "Password", "RegistrationDate", "UserId" },
                values: new object[] { new Guid("37dca5a0-83b6-4ace-bd89-e141670dd2d3"), "Admin", "Admin", new DateTime(2020, 12, 29, 8, 44, 39, 311, DateTimeKind.Local).AddTicks(2214), new Guid("91ed60a4-8a9d-404d-ad91-f067c1f2b8a5") });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCurrencies_AccountId",
                table: "AccountCurrencies",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountCurrencies_CurrencyName",
                table: "AccountCurrencies",
                column: "CurrencyName");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CurrencyName",
                table: "Operations",
                column: "CurrencyName");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_FromAccountId",
                table: "Operations",
                column: "FromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ToAccountId",
                table: "Operations",
                column: "ToAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommissions_CurrencyName",
                table: "UserCommissions",
                column: "CurrencyName");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommissions_UserId",
                table: "UserCommissions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountCurrencies");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "UserCommissions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "CurrencyInformation");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
