﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SampleProject.Infrastructure.Database;

#nullable disable

namespace Orders.Infrastructure.Migrations
{
    [DbContext(typeof(OrdersContext))]
    partial class OrdersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SampleProject.Domain.Customers.Orders.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(150)
                        .HasColumnType("NVarChar(150)")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("DateTime2")
                        .HasColumnName("order_date");

                    b.Property<string>("OrderNo")
                        .HasMaxLength(50)
                        .HasColumnType("NVarChar(50)")
                        .HasColumnName("order_no");

                    b.Property<decimal>("Price")
                        .HasPrecision(24, 2)
                        .HasColumnType("Decimal(24,2)")
                        .HasColumnName("price");

                    b.Property<string>("ProductName")
                        .HasMaxLength(100)
                        .HasColumnType("NVarChar(100)")
                        .HasColumnName("product_name");

                    b.Property<int>("Total")
                        .HasColumnType("Int")
                        .HasColumnName("total");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(24, 2)
                        .HasColumnType("Decimal(24,2)")
                        .HasColumnName("total_price");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("OrderNo")
                        .IsUnique()
                        .HasDatabaseName("ix_orders_order_no")
                        .HasFilter("[order_no] IS NOT NULL");

                    b.ToTable("orders", "orders");
                });

            modelBuilder.Entity("SampleProject.Domain.Customers.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .HasMaxLength(150)
                        .HasColumnType("NVarChar(150)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("NVarChar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_persons");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_persons_email")
                        .HasFilter("[email] IS NOT NULL");

                    b.ToTable("persons", "orders");
                });

            modelBuilder.Entity("SampleProject.Infrastructure.Processing.InternalCommands.InternalCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("data");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("processed_date");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_internal_commands");

                    b.ToTable("internal_commands", "app");
                });

            modelBuilder.Entity("SampleProject.Infrastructure.Processing.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("data");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("occurred_on");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("processed_date");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_outbox_messages");

                    b.ToTable("outbox_messages", "app");
                });
#pragma warning restore 612, 618
        }
    }
}
