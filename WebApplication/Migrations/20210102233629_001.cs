using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "currencies",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    deposit_commission = table.Column<decimal>(type: "numeric", nullable: false),
                    withdraw_commission = table.Column<decimal>(type: "numeric", nullable: false),
                    transfer_commission = table.Column<decimal>(type: "numeric", nullable: false),
                    deposit_limit = table.Column<decimal>(type: "numeric", nullable: false),
                    withdraw_limit = table.Column<decimal>(type: "numeric", nullable: false),
                    transfer_limit = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currencies", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<int>(type: "integer", nullable: false),
                    registration_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    registration_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounts", x => x.id);
                    table.ForeignKey(
                        name: "fk_accounts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_commissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    currency_name = table.Column<string>(type: "text", nullable: true),
                    deposit_commission = table.Column<decimal>(type: "numeric", nullable: true),
                    withdraw_commission = table.Column<decimal>(type: "numeric", nullable: true),
                    transfer_commission = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_commissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_commissions_currencies_currency_temp_id2",
                        column: x => x.currency_name,
                        principalTable: "currencies",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_commissions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_currencies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    currency_name = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_currencies", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_currencies_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_account_currencies_currencies_currency_temp_id",
                        column: x => x.currency_name,
                        principalTable: "currencies",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    to_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    currency_name = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_operations", x => x.id);
                    table.ForeignKey(
                        name: "fk_operations_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_operations_currencies_currency_temp_id1",
                        column: x => x.currency_name,
                        principalTable: "currencies",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "currencies",
                columns: new[] { "name", "deposit_commission", "deposit_limit", "transfer_commission", "transfer_limit", "withdraw_commission", "withdraw_limit" },
                values: new object[] { "USD", 10m, 1000m, 10m, 1000m, 10m, 1000m });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "registration_date", "role" },
                values: new object[] { new Guid("2145d43f-c46d-4fa6-912b-7fa7a983b5e8"), "Admin@com", new DateTime(2021, 1, 3, 2, 36, 28, 771, DateTimeKind.Local).AddTicks(6189), 0 });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "name", "password", "registration_date", "user_id" },
                values: new object[] { new Guid("b84ab5b5-94a6-4571-af02-59ef296543c0"), "Admin", "AQAAAAEAACcQAAAAEOTVMH6Ks/ifeiT/drv07DGNtzX2aBCKyZYZSJTaZhHIyM08fqIZdtA57I0JLbnH8A==", new DateTime(2021, 1, 3, 2, 36, 28, 786, DateTimeKind.Local).AddTicks(5101), new Guid("2145d43f-c46d-4fa6-912b-7fa7a983b5e8") });

            migrationBuilder.CreateIndex(
                name: "IX_account_currencies_account_id",
                table: "account_currencies",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_account_currencies_currency_name",
                table: "account_currencies",
                column: "currency_name");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_user_id",
                table: "accounts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_operations_account_id",
                table: "operations",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_operations_currency_name",
                table: "operations",
                column: "currency_name");

            migrationBuilder.CreateIndex(
                name: "IX_user_commissions_currency_name",
                table: "user_commissions",
                column: "currency_name");

            migrationBuilder.CreateIndex(
                name: "IX_user_commissions_user_id",
                table: "user_commissions",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_currencies");

            migrationBuilder.DropTable(
                name: "operations");

            migrationBuilder.DropTable(
                name: "user_commissions");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "currencies");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
