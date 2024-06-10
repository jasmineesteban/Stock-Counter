﻿// <auto-generated />
using System;
using MAUI.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MAUI.API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240610040120_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("MAUI.API.Data.Entities.CountItems", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("CountSheetId")
                        .HasColumnType("bigint");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<long?>("SheetId")
                        .HasColumnType("bigint");

                    b.Property<string>("UOM")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("SheetId");

                    b.ToTable("CountItems");
                });

            modelBuilder.Entity("MAUI.API.Data.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Expiry")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<double>("InitialCount")
                        .HasMaxLength(45)
                        .HasColumnType("double");

                    b.Property<string>("InventoryDate")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("Lot")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<double>("QuantityCount")
                        .HasMaxLength(45)
                        .HasColumnType("double");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("Items");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Expiry = "0000-00-00",
                            InitialCount = 1.0,
                            InventoryDate = "0000-00-00",
                            ItemName = "ItemA",
                            Lot = "0000-00-00",
                            QuantityCount = 10.0,
                            User = "Employee A"
                        },
                        new
                        {
                            Id = 2,
                            Expiry = "0000-00-00",
                            InitialCount = 1.0,
                            InventoryDate = "0000-00-00",
                            ItemName = "ItemB",
                            Lot = "0000-00-00",
                            QuantityCount = 10.0,
                            User = "Employee A"
                        });
                });

            modelBuilder.Entity("MAUI.API.Data.Entities.ItemOption", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("UOM")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("ItemId", "UOM");

                    b.ToTable("ItemOptions");

                    b.HasData(
                        new
                        {
                            ItemId = 1,
                            UOM = "Pcs",
                            Id = 0
                        },
                        new
                        {
                            ItemId = 1,
                            UOM = "Box",
                            Id = 0
                        });
                });

            modelBuilder.Entity("MAUI.API.Data.Entities.Sheet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CountAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("char(36)");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<long>("StockCountSheetId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Sheets");
                });

            modelBuilder.Entity("MAUI.API.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(180)
                        .HasColumnType("varchar(180)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MAUI.API.Data.Entities.CountItems", b =>
                {
                    b.HasOne("MAUI.API.Data.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MAUI.API.Data.Entities.Sheet", null)
                        .WithMany("CountItems")
                        .HasForeignKey("SheetId");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("MAUI.API.Data.Entities.Item", b =>
                {
                    b.HasOne("MAUI.API.Data.Entities.Item", null)
                        .WithMany("ItemOptions")
                        .HasForeignKey("ItemId");
                });

            modelBuilder.Entity("MAUI.API.Data.Entities.ItemOption", b =>
                {
                    b.HasOne("MAUI.API.Data.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("MAUI.API.Data.Entities.Item", b =>
                {
                    b.Navigation("ItemOptions");
                });

            modelBuilder.Entity("MAUI.API.Data.Entities.Sheet", b =>
                {
                    b.Navigation("CountItems");
                });
#pragma warning restore 612, 618
        }
    }
}
