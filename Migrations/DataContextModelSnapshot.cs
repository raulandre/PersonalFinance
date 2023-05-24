﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PersonalFinance.Data;

#nullable disable

namespace PersonalFinance.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("PersonalFinance.Models.Balance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<decimal>("Salary")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("PersonalFinance.Models.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("BalanceId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("ExpenseType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BalanceId");

                    b.ToTable("Expense");
                });

            modelBuilder.Entity("PersonalFinance.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("BalanceId")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<byte?>("Onboarding")
                        .HasColumnType("smallint");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BalanceId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PersonalFinance.Models.Expense", b =>
                {
                    b.HasOne("PersonalFinance.Models.Balance", "Balance")
                        .WithMany("Expenses")
                        .HasForeignKey("BalanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Balance");
                });

            modelBuilder.Entity("PersonalFinance.Models.User", b =>
                {
                    b.HasOne("PersonalFinance.Models.Balance", "Balance")
                        .WithOne("User")
                        .HasForeignKey("PersonalFinance.Models.User", "BalanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Balance");
                });

            modelBuilder.Entity("PersonalFinance.Models.Balance", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
