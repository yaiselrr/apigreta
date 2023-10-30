using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class Store : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseBottleRefund",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseDiscount",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseExchange",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseGiftCards",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseNoSale",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsePaidOut",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseReturn",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseTaxOverride",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseZeroScale",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseBottleRefund",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UseDiscount",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UseExchange",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UseGiftCards",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UseNoSale",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UsePaidOut",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UseReturn",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UseTaxOverride",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UseZeroScale",
                table: "Store");
        }
    }
}
