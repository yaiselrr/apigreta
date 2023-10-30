using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class reajustbinlocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BinLocation",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "OrderAmount",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OrderTrigger",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "QtyHand",
                table: "Product");

            migrationBuilder.AddColumn<decimal>(
                name: "OrderAmount",
                table: "StoreProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderTrigger",
                table: "StoreProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyHand",
                table: "StoreProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderAmount",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "OrderTrigger",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "QtyHand",
                table: "StoreProduct");

            migrationBuilder.AddColumn<string>(
                name: "BinLocation",
                table: "StoreProduct",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderAmount",
                table: "Product",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderTrigger",
                table: "Product",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyHand",
                table: "Product",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
