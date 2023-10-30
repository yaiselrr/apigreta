using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class adddingcascadetocutlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CutListDetail_CutList_CutListId",
                table: "CutListDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_CutListDetail_CutList_CutListId",
                table: "CutListDetail",
                column: "CutListId",
                principalTable: "CutList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            
            migrationBuilder.DropTable(
                name: "DepartmentOnlineStore");
            migrationBuilder.DropTable(
                name: "OnlineStore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CutListDetail_CutList_CutListId",
                table: "CutListDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_CutListDetail_CutList_CutListId",
                table: "CutListDetail",
                column: "CutListId",
                principalTable: "CutList",
                principalColumn: "Id");
        }
    }
}
