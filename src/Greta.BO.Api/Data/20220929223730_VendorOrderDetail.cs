using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class VendorOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorOrderDetail_Product_VendorOrderId",
                table: "VendorOrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_VendorOrderDetail_ProductId",
                table: "VendorOrderDetail",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorOrderDetail_Product_ProductId",
                table: "VendorOrderDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorOrderDetail_Product_ProductId",
                table: "VendorOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_VendorOrderDetail_ProductId",
                table: "VendorOrderDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorOrderDetail_Product_VendorOrderId",
                table: "VendorOrderDetail",
                column: "VendorOrderId",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
