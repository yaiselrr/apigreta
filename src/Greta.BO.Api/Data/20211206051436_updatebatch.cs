using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class updatebatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batch_Store_StoreId",
                table: "Batch");

            migrationBuilder.DropIndex(
                name: "IX_Batch_StoreId",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Batch");

            migrationBuilder.CreateTable(
                name: "BatchStore",
                columns: table => new
                {
                    BatchsId = table.Column<long>(type: "bigint", nullable: false),
                    StoresId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchStore", x => new { x.BatchsId, x.StoresId });
                    table.ForeignKey(
                        name: "FK_BatchStore_Batch_BatchsId",
                        column: x => x.BatchsId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchStore_Store_StoresId",
                        column: x => x.StoresId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchStore_StoresId",
                table: "BatchStore",
                column: "StoresId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchStore");

            migrationBuilder.AddColumn<long>(
                name: "StoreId",
                table: "StoreId",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Batch_StoreId",
                table: "Batch",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batch_Store_StoreId",
                table: "Batch",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


        }
    }
}
