using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class addingsaleandproductnewfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostCode",
                table: "SaleTender",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostDetailedMessage",
                table: "SaleTender",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostResponse",
                table: "SaleTender",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsedName",
                table: "SaleProduct",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsedUPC",
                table: "SaleProduct",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name1",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name2",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name3",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UPC1",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UPC2",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UPC3",
                table: "Product",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostCode",
                table: "SaleTender");

            migrationBuilder.DropColumn(
                name: "HostDetailedMessage",
                table: "SaleTender");

            migrationBuilder.DropColumn(
                name: "HostResponse",
                table: "SaleTender");

            migrationBuilder.DropColumn(
                name: "UsedName",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "UsedUPC",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "Name1",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Name2",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Name3",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UPC1",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UPC2",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UPC3",
                table: "Product");
        }
    }
}
