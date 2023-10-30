using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addmixandmatchglobalstorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MixAndMatchGlobalDiscount",
                table: "SaleProduct",
                type: "numeric(18,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SaleProductDiscountApplied",
                table: "SaleProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MixAndMatchGlobalDiscount",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "SaleProductDiscountApplied",
                table: "SaleProduct");
        }
    }
}
