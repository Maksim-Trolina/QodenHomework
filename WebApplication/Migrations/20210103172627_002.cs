using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_operations_accounts_account_id",
                table: "operations");

            migrationBuilder.DropIndex(
                name: "IX_operations_account_id",
                table: "operations");

            migrationBuilder.DeleteData(
                table: "accounts",
                keyColumn: "id",
                keyValue: new Guid("b84ab5b5-94a6-4571-af02-59ef296543c0"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("2145d43f-c46d-4fa6-912b-7fa7a983b5e8"));

            migrationBuilder.DropColumn(
                name: "account_id",
                table: "operations");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "registration_date", "role" },
                values: new object[] { new Guid("07148ffd-69c9-4502-a5af-57e97e8d7206"), "Admin@com", new DateTime(2021, 1, 3, 20, 26, 26, 580, DateTimeKind.Local).AddTicks(4793), 0 });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "name", "password", "registration_date", "user_id" },
                values: new object[] { new Guid("ad1ef448-d663-4407-936f-8a97e01c0fea"), "Admin", "AQAAAAEAACcQAAAAEL2WB5CfUjUnIvjLyFi2TOvfN98Nv/AoaTkbFUDmkuSUh34s/TuN5CwuLk+M0j3JCw==", new DateTime(2021, 1, 3, 20, 26, 26, 594, DateTimeKind.Local).AddTicks(7009), new Guid("07148ffd-69c9-4502-a5af-57e97e8d7206") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "accounts",
                keyColumn: "id",
                keyValue: new Guid("ad1ef448-d663-4407-936f-8a97e01c0fea"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("07148ffd-69c9-4502-a5af-57e97e8d7206"));

            migrationBuilder.AddColumn<Guid>(
                name: "account_id",
                table: "operations",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "registration_date", "role" },
                values: new object[] { new Guid("2145d43f-c46d-4fa6-912b-7fa7a983b5e8"), "Admin@com", new DateTime(2021, 1, 3, 2, 36, 28, 771, DateTimeKind.Local).AddTicks(6189), 0 });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "name", "password", "registration_date", "user_id" },
                values: new object[] { new Guid("b84ab5b5-94a6-4571-af02-59ef296543c0"), "Admin", "AQAAAAEAACcQAAAAEOTVMH6Ks/ifeiT/drv07DGNtzX2aBCKyZYZSJTaZhHIyM08fqIZdtA57I0JLbnH8A==", new DateTime(2021, 1, 3, 2, 36, 28, 786, DateTimeKind.Local).AddTicks(5101), new Guid("2145d43f-c46d-4fa6-912b-7fa7a983b5e8") });

            migrationBuilder.CreateIndex(
                name: "IX_operations_account_id",
                table: "operations",
                column: "account_id");

            migrationBuilder.AddForeignKey(
                name: "fk_operations_accounts_account_id",
                table: "operations",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
