using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class fixdriverproblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_SaleDriverLicense_DriverLicenseId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Sale_DriverLicenseId",
                table: "Sale");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDriverLicense_SaleId",
                table: "SaleDriverLicense",
                column: "SaleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDriverLicense_Sale_SaleId",
                table: "SaleDriverLicense",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDriverLicense_Sale_SaleId",
                table: "SaleDriverLicense");

            migrationBuilder.DropIndex(
                name: "IX_SaleDriverLicense_SaleId",
                table: "SaleDriverLicense");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_DriverLicenseId",
                table: "Sale",
                column: "DriverLicenseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_SaleDriverLicense_DriverLicenseId",
                table: "Sale",
                column: "DriverLicenseId",
                principalTable: "SaleDriverLicense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
