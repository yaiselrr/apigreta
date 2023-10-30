using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class changePackSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizePack",
                table: "VendorOrderDetail");

            migrationBuilder.AlterColumn<string>(
                name: "PackSize",
                table: "VendorProduct",
                type: "text",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "PackSize",
                table: "VendorOrderDetail",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackSize",
                table: "VendorOrderDetail");

            migrationBuilder.AlterColumn<decimal>(
                name: "PackSize",
                table: "VendorProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SizePack",
                table: "VendorOrderDetail",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
