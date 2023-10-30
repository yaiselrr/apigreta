using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class ajustsalesdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RawResponse",
                table: "SaleTender",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                table: "SaleProduct",
                type: "numeric(18,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Subtitle",
                table: "SaleProduct",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SubtitleVisible",
                table: "SaleProduct",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ClearCashDiscountTotal",
                table: "Sale",
                type: "numeric(18,3)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawResponse",
                table: "SaleTender");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "Subtitle",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "SubtitleVisible",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "ClearCashDiscountTotal",
                table: "Sale");
        }
    }
}
