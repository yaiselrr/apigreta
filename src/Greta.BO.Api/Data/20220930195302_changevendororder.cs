using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class changevendororder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "VendorOrder");

            migrationBuilder.DropColumn(
                name: "Prepared",
                table: "VendorOrder");

            migrationBuilder.AddColumn<decimal>(
                name: "RecivedAmount",
                table: "VendorOrderDetail",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "VendorOrder",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecivedAmount",
                table: "VendorOrderDetail");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "VendorOrder");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "VendorOrder",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Prepared",
                table: "VendorOrder",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
