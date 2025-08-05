using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelzEcommerce.Modules.Orders.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "order");

        migrationBuilder.CreateTable(
            name: "orders",
            schema: "order",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(550)", maxLength: 550, nullable: false),
                status = table.Column<int>(type: "integer", nullable: false),
                total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_orders", x => x.id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "orders",
            schema: "order");
    }
}
