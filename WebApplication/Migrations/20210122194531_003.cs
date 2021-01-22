using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class _003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "accounts",
                keyColumn: "id",
                keyValue: new Guid("ad1ef448-d663-4407-936f-8a97e01c0fea"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("07148ffd-69c9-4502-a5af-57e97e8d7206"));

            migrationBuilder.RenameColumn(
                name: "withdraw_commission",
                table: "user_commissions",
                newName: "withdraw_relative_commission");

            migrationBuilder.RenameColumn(
                name: "transfer_commission",
                table: "user_commissions",
                newName: "transfer_relative_commission");

            migrationBuilder.RenameColumn(
                name: "deposit_commission",
                table: "user_commissions",
                newName: "deposit_relative_commission");

            migrationBuilder.RenameColumn(
                name: "withdraw_commission",
                table: "currencies",
                newName: "withdraw_relative_commission");

            migrationBuilder.RenameColumn(
                name: "transfer_commission",
                table: "currencies",
                newName: "transfer_relative_commission");

            migrationBuilder.RenameColumn(
                name: "deposit_commission",
                table: "currencies",
                newName: "deposit_relative_commission");

            migrationBuilder.AddColumn<decimal>(
                name: "deposit_absolute_commission",
                table: "currencies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "transfer_absolute_commission",
                table: "currencies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "withdraw_absolute_commission",
                table: "currencies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "currencies",
                keyColumn: "name",
                keyValue: "USD",
                columns: new[] { "deposit_absolute_commission", "deposit_relative_commission", "transfer_absolute_commission", "transfer_relative_commission", "withdraw_absolute_commission", "withdraw_relative_commission" },
                values: new object[] { 100m, 10m, 100m, 10m, 100m, 10m });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "registration_date", "role" },
                values: new object[] { new Guid("bdcd8c62-8a9c-4343-b7d0-f9785959686b"), "Admin@com", new DateTime(2021, 1, 22, 22, 45, 30, 672, DateTimeKind.Local).AddTicks(471), 0 });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "name", "password", "registration_date", "user_id" },
                values: new object[] { new Guid("260af1c4-921a-413d-a6b7-f4e32da9e885"), "Admin", "AQAAAAEAACcQAAAAEKN7pdphB+Wj4dPvL7rOf9J+XqwW9t+vGjuT2HxpWaM9QtSYkQQZpxWgDXxvje3kUQ==", new DateTime(2021, 1, 22, 22, 45, 30, 875, DateTimeKind.Local).AddTicks(4012), new Guid("bdcd8c62-8a9c-4343-b7d0-f9785959686b") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "accounts",
                keyColumn: "id",
                keyValue: new Guid("260af1c4-921a-413d-a6b7-f4e32da9e885"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("bdcd8c62-8a9c-4343-b7d0-f9785959686b"));

            migrationBuilder.DropColumn(
                name: "deposit_absolute_commission",
                table: "currencies");

            migrationBuilder.DropColumn(
                name: "transfer_absolute_commission",
                table: "currencies");

            migrationBuilder.DropColumn(
                name: "withdraw_absolute_commission",
                table: "currencies");

            migrationBuilder.RenameColumn(
                name: "withdraw_relative_commission",
                table: "user_commissions",
                newName: "withdraw_commission");

            migrationBuilder.RenameColumn(
                name: "transfer_relative_commission",
                table: "user_commissions",
                newName: "transfer_commission");

            migrationBuilder.RenameColumn(
                name: "deposit_relative_commission",
                table: "user_commissions",
                newName: "deposit_commission");

            migrationBuilder.RenameColumn(
                name: "withdraw_relative_commission",
                table: "currencies",
                newName: "withdraw_commission");

            migrationBuilder.RenameColumn(
                name: "transfer_relative_commission",
                table: "currencies",
                newName: "transfer_commission");

            migrationBuilder.RenameColumn(
                name: "deposit_relative_commission",
                table: "currencies",
                newName: "deposit_commission");

            migrationBuilder.UpdateData(
                table: "currencies",
                keyColumn: "name",
                keyValue: "USD",
                columns: new[] { "deposit_commission", "transfer_commission", "withdraw_commission" },
                values: new object[] { 0m, 0m, 0m });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "registration_date", "role" },
                values: new object[] { new Guid("07148ffd-69c9-4502-a5af-57e97e8d7206"), "Admin@com", new DateTime(2021, 1, 3, 20, 26, 26, 580, DateTimeKind.Local).AddTicks(4793), 0 });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "name", "password", "registration_date", "user_id" },
                values: new object[] { new Guid("ad1ef448-d663-4407-936f-8a97e01c0fea"), "Admin", "AQAAAAEAACcQAAAAEL2WB5CfUjUnIvjLyFi2TOvfN98Nv/AoaTkbFUDmkuSUh34s/TuN5CwuLk+M0j3JCw==", new DateTime(2021, 1, 3, 20, 26, 26, 594, DateTimeKind.Local).AddTicks(7009), new Guid("07148ffd-69c9-4502-a5af-57e97e8d7206") });
        }
    }
}
