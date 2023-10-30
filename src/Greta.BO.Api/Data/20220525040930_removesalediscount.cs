using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class removesalediscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleDiscount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaleDiscount",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ApplyAutomatically = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyToCustomerOnly = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyToProduct = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    SaleProductId = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Value = table.Column<decimal>(type: "numeric(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDiscount_SaleProduct_SaleProductId",
                        column: x => x.SaleProductId,
                        principalTable: "SaleProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleDiscount_SaleProductId",
                table: "SaleDiscount",
                column: "SaleProductId",
                unique: true);
        }
    }
}
