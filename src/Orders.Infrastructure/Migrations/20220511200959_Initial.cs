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
                name: "InternalCommands",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_internal_commands", x => x.Id);
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
                name: "OutboxMessages",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.Id);
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
                name: "InternalCommands",
                schema: "app");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "app");

            migrationBuilder.DropTable(
                name: "persons",
                schema: "orders");
        }
    }
}
