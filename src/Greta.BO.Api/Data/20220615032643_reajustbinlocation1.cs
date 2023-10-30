using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class reajustbinlocation1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BinLocationId",
                table: "StoreProduct",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreProduct_BinLocationId",
                table: "StoreProduct",
                column: "BinLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BinLocation_Id",
                table: "BinLocation",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreProduct_BinLocation_BinLocationId",
                table: "StoreProduct",
                column: "BinLocationId",
                principalTable: "BinLocation",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreProduct_BinLocation_BinLocationId",
                table: "StoreProduct");

            migrationBuilder.DropIndex(
                name: "IX_StoreProduct_BinLocationId",
                table: "StoreProduct");

            migrationBuilder.DropIndex(
                name: "IX_BinLocation_Id",
                table: "BinLocation");

            migrationBuilder.DropColumn(
                name: "BinLocationId",
                table: "StoreProduct");
        }
    }
}
