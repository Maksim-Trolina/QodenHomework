﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication.Database;

namespace WebApplication.Migrations
{
    [DbContext(typeof(Db))]
    [Migration("20210102233629_001")]
    partial class _001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("WebApplication.Database.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("registration_date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_accounts");

                    b.HasIndex("UserId");

                    b.ToTable("accounts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b84ab5b5-94a6-4571-af02-59ef296543c0"),
                            Name = "Admin",
                            Password = "AQAAAAEAACcQAAAAEOTVMH6Ks/ifeiT/drv07DGNtzX2aBCKyZYZSJTaZhHIyM08fqIZdtA57I0JLbnH8A==",
                            RegistrationDate = new DateTime(2021, 1, 3, 2, 36, 28, 786, DateTimeKind.Local).AddTicks(5101),
                            UserId = new Guid("2145d43f-c46d-4fa6-912b-7fa7a983b5e8")
                        });
                });

            modelBuilder.Entity("WebApplication.Database.Models.AccountCurrency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<string>("CurrencyName")
                        .HasColumnType("text")
                        .HasColumnName("currency_name");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_account_currencies");

                    b.HasIndex("AccountId");

                    b.HasIndex("CurrencyName");

                    b.ToTable("account_currencies");
                });

            modelBuilder.Entity("WebApplication.Database.Models.Currency", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<decimal>("DepositRelativeCommission")
                        .HasColumnType("numeric")
                        .HasColumnName("deposit_commission");

                    b.Property<decimal>("DepositLimit")
                        .HasColumnType("numeric")
                        .HasColumnName("deposit_limit");

                    b.Property<decimal>("TransferRelativeCommission")
                        .HasColumnType("numeric")
                        .HasColumnName("transfer_commission");

                    b.Property<decimal>("TransferLimit")
                        .HasColumnType("numeric")
                        .HasColumnName("transfer_limit");

                    b.Property<decimal>("WithdrawRelativeCommission")
                        .HasColumnType("numeric")
                        .HasColumnName("withdraw_commission");

                    b.Property<decimal>("WithdrawLimit")
                        .HasColumnType("numeric")
                        .HasColumnName("withdraw_limit");

                    b.HasKey("Name");

                    b.ToTable("currencies");

                    b.HasData(
                        new
                        {
                            Name = "USD",
                            DepositCommission = 10m,
                            DepositLimit = 1000m,
                            TransferCommission = 10m,
                            TransferLimit = 1000m,
                            WithdrawCommission = 10m,
                            WithdrawLimit = 1000m
                        });
                });

            modelBuilder.Entity("WebApplication.Database.Models.Operation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<string>("CurrencyName")
                        .HasColumnType("text")
                        .HasColumnName("currency_name");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date");

                    b.Property<Guid>("FromAccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("from_account_id");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid>("ToAccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("to_account_id");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_operations");

                    b.HasIndex("AccountId");

                    b.HasIndex("CurrencyName");

                    b.ToTable("operations");
                });

            modelBuilder.Entity("WebApplication.Database.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("registration_date");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("2145d43f-c46d-4fa6-912b-7fa7a983b5e8"),
                            Email = "Admin@com",
                            RegistrationDate = new DateTime(2021, 1, 3, 2, 36, 28, 771, DateTimeKind.Local).AddTicks(6189),
                            Role = 0
                        });
                });

            modelBuilder.Entity("WebApplication.Database.Models.UserCommission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CurrencyName")
                        .HasColumnType("text")
                        .HasColumnName("currency_name");

                    b.Property<decimal?>("DepositRelativeCommission")
                        .HasColumnType("numeric")
                        .HasColumnName("deposit_commission");

                    b.Property<decimal?>("TransferRelativeCommission")
                        .HasColumnType("numeric")
                        .HasColumnName("transfer_commission");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<decimal?>("WithdrawRelativeCommission")
                        .HasColumnType("numeric")
                        .HasColumnName("withdraw_commission");

                    b.HasKey("Id")
                        .HasName("pk_user_commissions");

                    b.HasIndex("CurrencyName");

                    b.HasIndex("UserId");

                    b.ToTable("user_commissions");
                });

            modelBuilder.Entity("WebApplication.Database.Models.Account", b =>
                {
                    b.HasOne("WebApplication.Database.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_accounts_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApplication.Database.Models.AccountCurrency", b =>
                {
                    b.HasOne("WebApplication.Database.Models.Account", "Account")
                        .WithMany("AccountCurrencies")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("fk_account_currencies_accounts_account_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Database.Models.Currency", "Currency")
                        .WithMany("AccountCurrencies")
                        .HasForeignKey("CurrencyName")
                        .HasConstraintName("fk_account_currencies_currencies_currency_temp_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Account");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("WebApplication.Database.Models.Operation", b =>
                {
                    b.HasOne("WebApplication.Database.Models.Account", null)
                        .WithMany("Operations")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("fk_operations_accounts_account_id");

                    b.HasOne("WebApplication.Database.Models.Currency", "Currency")
                        .WithMany("Operations")
                        .HasForeignKey("CurrencyName")
                        .HasConstraintName("fk_operations_currencies_currency_temp_id1")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("WebApplication.Database.Models.UserCommission", b =>
                {
                    b.HasOne("WebApplication.Database.Models.Currency", "Currency")
                        .WithMany("UserCommissions")
                        .HasForeignKey("CurrencyName")
                        .HasConstraintName("fk_user_commissions_currencies_currency_temp_id2")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApplication.Database.Models.User", "User")
                        .WithMany("UserCommissions")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_commissions_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApplication.Database.Models.Account", b =>
                {
                    b.Navigation("AccountCurrencies");

                    b.Navigation("Operations");
                });

            modelBuilder.Entity("WebApplication.Database.Models.Currency", b =>
                {
                    b.Navigation("AccountCurrencies");

                    b.Navigation("Operations");

                    b.Navigation("UserCommissions");
                });

            modelBuilder.Entity("WebApplication.Database.Models.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("UserCommissions");
                });
#pragma warning restore 612, 618
        }
    }
}