using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class SomeNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PackSize",
                table: "VendorProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SizePack",
                table: "VendorOrderDetail",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "LiquorLicenseId",
                table: "Store",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxPayerId",
                table: "Store",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLiquorCategory",
                table: "Category",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackSize",
                table: "VendorProduct");

            migrationBuilder.DropColumn(
                name: "SizePack",
                table: "VendorOrderDetail");

            migrationBuilder.DropColumn(
                name: "LiquorLicenseId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "TaxPayerId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "IsLiquorCategory",
                table: "Category");
        }
    }
}
