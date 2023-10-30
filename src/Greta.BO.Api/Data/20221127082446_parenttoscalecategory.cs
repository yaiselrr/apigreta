using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class parenttoscalecategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "ScaleCategory",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScaleCategory_ParentId",
                table: "ScaleCategory",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScaleCategory_ScaleCategory_ParentId",
                table: "ScaleCategory",
                column: "ParentId",
                principalTable: "ScaleCategory",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScaleCategory_ScaleCategory_ParentId",
                table: "ScaleCategory");

            migrationBuilder.DropIndex(
                name: "IX_ScaleCategory_ParentId",
                table: "ScaleCategory");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ScaleCategory");
        }
    }
}
