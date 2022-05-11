using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.CreateTable(
                name: "internal_commands",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    processed_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_internal_commands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_date = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    created_by = table.Column<string>(type: "NVarChar(150)", maxLength: 150, nullable: true),
                    order_no = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    product_name = table.Column<string>(type: "NVarChar(100)", maxLength: 100, nullable: true),
                    total = table.Column<int>(type: "Int", nullable: false),
                    price = table.Column<decimal>(type: "Decimal(24,2)", precision: 24, scale: 2, nullable: false),
                    total_price = table.Column<decimal>(type: "Decimal(24,2)", precision: 24, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    occurred_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    processed_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "persons",
                schema: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "NVarChar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_persons", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_orders_order_no",
                schema: "orders",
                table: "orders",
                column: "order_no",
                unique: true,
                filter: "[order_no] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_persons_email",
                schema: "orders",
                table: "persons",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "internal_commands",
                schema: "app");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "app");

            migrationBuilder.DropTable(
                name: "persons",
                schema: "orders");
        }
    }
}
