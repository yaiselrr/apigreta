using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class storeconfiguses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseEBTCheckBalance",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseRemoveserviceFee",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseReprintReceipt",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseEBTCheckBalance",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UseRemoveserviceFee",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UseReprintReceipt",
                table: "Store");
        }
    }
}
