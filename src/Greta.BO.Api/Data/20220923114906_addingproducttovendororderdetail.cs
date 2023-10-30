using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addingproducttovendororderdetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "VendorOrderDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorOrderDetail_Product_VendorOrderId",
                table: "VendorOrderDetail",
                column: "VendorOrderId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorOrderDetail_Product_VendorOrderId",
                table: "VendorOrderDetail");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "VendorOrderDetail");
        }
    }
}
