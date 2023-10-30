using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class saleproductupdatediscountinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "SaleProduct",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "DiscountApplyAutomatically",
                table: "SaleProduct",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DiscountApplyToCustomerOnly",
                table: "SaleProduct",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DiscountApplyToProduct",
                table: "SaleProduct",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DiscountName",
                table: "SaleProduct",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "SaleProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "DiscountApplyAutomatically",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "DiscountApplyToCustomerOnly",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "DiscountApplyToProduct",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "DiscountName",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "SaleProduct");
        }
    }
}
