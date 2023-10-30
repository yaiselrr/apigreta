using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addParentAndChildSupportToStoreProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChildId",
                table: "StoreProduct",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "StoreProduct",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreProduct_ParentId",
                table: "StoreProduct",
                column: "ParentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreProduct_StoreProduct_ParentId",
                table: "StoreProduct",
                column: "ParentId",
                principalTable: "StoreProduct",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreProduct_StoreProduct_ParentId",
                table: "StoreProduct");

            migrationBuilder.DropIndex(
                name: "IX_StoreProduct_ParentId",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "StoreProduct");
        }
    }
}
