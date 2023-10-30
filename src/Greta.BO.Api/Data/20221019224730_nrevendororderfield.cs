using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class nrevendororderfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "VendorOrderDetail",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UPC",
                table: "VendorOrderDetail",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "VendorOrderDetail");

            migrationBuilder.DropColumn(
                name: "UPC",
                table: "VendorOrderDetail");
        }
    }
}
