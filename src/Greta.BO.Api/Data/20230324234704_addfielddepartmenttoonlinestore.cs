using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addfielddepartmenttoonlinestore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentOnlineStore",
                columns: table => new
                {
                    DepartmentsId = table.Column<long>(type: "bigint", nullable: false),
                    OnlineStoresId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentOnlineStore", x => new { x.DepartmentsId, x.OnlineStoresId });
                    table.ForeignKey(
                        name: "FK_DepartmentOnlineStore_Department_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentOnlineStore_OnlineStore_OnlineStoresId",
                        column: x => x.OnlineStoresId,
                        principalTable: "OnlineStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOnlineStore_OnlineStoresId",
                table: "DepartmentOnlineStore",
                column: "OnlineStoresId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentOnlineStore");
        }
    }
}
