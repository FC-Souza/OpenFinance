﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebAPI.OpenFinance.Data;

#nullable disable

namespace WebAPI.OpenFinance.Migrations
{
    [DbContext(typeof(OpenFinanceContext))]
    partial class OpenFinanceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebAPI.OpenFinance.Models.BanksModel", b =>
                {
                    b.Property<int>("bankID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("bank_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("bankID"));

                    b.Property<string>("bankName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("bank_name");

                    b.HasKey("bankID");

                    b.ToTable("banks", (string)null);
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.CashInfoModel", b =>
                {
                    b.Property<int>("cashInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("cash_info_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("cashInfoId"));

                    b.Property<decimal>("amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<int>("cashId")
                        .HasColumnType("integer")
                        .HasColumnName("cash_id");

                    b.Property<int>("connectionId")
                        .HasColumnType("integer")
                        .HasColumnName("connection_id");

                    b.HasKey("cashInfoId");

                    b.HasIndex("cashId");

                    b.HasIndex("connectionId");

                    b.ToTable("cash_info", (string)null);
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.CashModel", b =>
                {
                    b.Property<int>("cashId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("cash_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("cashId"));

                    b.Property<string>("cashDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("cash_description");

                    b.Property<int>("productId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<int?>("product_types")
                        .HasColumnType("integer");

                    b.HasKey("cashId");

                    b.HasIndex("product_types");

                    b.ToTable("cash", (string)null);
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.ClientCredentialModel", b =>
                {
                    b.Property<int>("clientCredentialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("client_credential_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("clientCredentialId"));

                    b.Property<int>("clientId")
                        .HasColumnType("integer")
                        .HasColumnName("client_id");

                    b.Property<string>("clientPassword")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("client_password");

                    b.Property<DateTime?>("lastLogin")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_login");

                    b.Property<int>("remainingLoginAttempts")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(3)
                        .HasColumnName("remaining_login_attempts");

                    b.HasKey("clientCredentialId");

                    b.HasIndex("clientId");

                    b.ToTable("client_credential", (string)null);
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.ClientsModel", b =>
                {
                    b.Property<int>("clientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("client_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("clientID"));

                    b.Property<string>("clientAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("client_address");

                    b.Property<string>("clientEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("client_email");

                    b.Property<string>("clientName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("client_name");

                    b.HasKey("clientID");

                    b.ToTable("clients", (string)null);
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.ConnectionsModel", b =>
                {
                    b.Property<int>("connectionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("connection_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("connectionID"));

                    b.Property<int>("accountNumber")
                        .HasColumnType("integer")
                        .HasColumnName("account_number");

                    b.Property<int>("bankID")
                        .HasColumnType("integer")
                        .HasColumnName("bank_id");

                    b.Property<int>("clientID")
                        .HasColumnType("integer")
                        .HasColumnName("client_id");

                    b.HasKey("connectionID");

                    b.HasIndex("bankID");

                    b.HasIndex("clientID");

                    b.ToTable("connections", (string)null);
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.ProductTypesModel", b =>
                {
                    b.Property<int>("productId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("productId"));

                    b.Property<string>("productDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("product_description");

                    b.Property<string>("productType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("product_type");

                    b.HasKey("productId");

                    b.ToTable("product_types", (string)null);
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.StockInfoModel", b =>
                {
                    b.Property<int>("stockInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("stock_info_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("stockInfoId"));

                    b.Property<decimal>("averagePrice")
                        .HasColumnType("numeric")
                        .HasColumnName("average_price");

                    b.Property<int>("connectionId")
                        .HasColumnType("integer")
                        .HasColumnName("connection_id");

                    b.Property<DateTime>("lastUpdated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_updated");

                    b.Property<int>("quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<int>("stockId")
                        .HasColumnType("integer")
                        .HasColumnName("stock_id");

                    b.HasKey("stockInfoId");

                    b.HasIndex("connectionId");

                    b.HasIndex("stockId");

                    b.ToTable("stock_info", (string)null);
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.StockModel", b =>
                {
                    b.Property<int>("stockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("stock_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("stockId"));

                    b.Property<string>("ISIN")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("isin");

                    b.Property<string>("currency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency");

                    b.Property<string>("exchange")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("exchange");

                    b.Property<decimal>("lastDayPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("last_day_price");

                    b.Property<DateTime>("lastUpdated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_updated");

                    b.Property<int>("productId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<int?>("product_types")
                        .HasColumnType("integer");

                    b.Property<string>("stockName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("stock_name");

                    b.Property<string>("ticker")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ticker");

                    b.HasKey("stockId");

                    b.HasIndex("product_types");

                    b.ToTable("stock", (string)null);
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.CashInfoModel", b =>
                {
                    b.HasOne("WebAPI.OpenFinance.Models.CashModel", "Cash")
                        .WithMany()
                        .HasForeignKey("cashId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI.OpenFinance.Models.ConnectionsModel", "Connection")
                        .WithMany()
                        .HasForeignKey("connectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cash");

                    b.Navigation("Connection");
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.CashModel", b =>
                {
                    b.HasOne("WebAPI.OpenFinance.Models.ProductTypesModel", "Product")
                        .WithMany()
                        .HasForeignKey("product_types");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.ClientCredentialModel", b =>
                {
                    b.HasOne("WebAPI.OpenFinance.Models.ClientsModel", "Client")
                        .WithMany()
                        .HasForeignKey("clientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.ConnectionsModel", b =>
                {
                    b.HasOne("WebAPI.OpenFinance.Models.BanksModel", "Bank")
                        .WithMany()
                        .HasForeignKey("bankID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI.OpenFinance.Models.ClientsModel", "Client")
                        .WithMany()
                        .HasForeignKey("clientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.StockInfoModel", b =>
                {
                    b.HasOne("WebAPI.OpenFinance.Models.ConnectionsModel", "Connection")
                        .WithMany()
                        .HasForeignKey("connectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI.OpenFinance.Models.StockModel", "Stock")
                        .WithMany()
                        .HasForeignKey("stockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Connection");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("WebAPI.OpenFinance.Models.StockModel", b =>
                {
                    b.HasOne("WebAPI.OpenFinance.Models.ProductTypesModel", "Product")
                        .WithMany()
                        .HasForeignKey("product_types");

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
