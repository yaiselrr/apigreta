using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class fixsaletaxrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleFee_Sale_SaleId",
                table: "SaleFee");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleTax_Sale_SaleId",
                table: "SaleTax");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleFee_Sale_SaleId",
                table: "SaleFee",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleTax_Sale_SaleId",
                table: "SaleTax",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleFee_Sale_SaleId",
                table: "SaleFee");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleTax_Sale_SaleId",
                table: "SaleTax");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleFee_Sale_SaleId",
                table: "SaleFee",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleTax_Sale_SaleId",
                table: "SaleTax",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id");
        }
    }
}
