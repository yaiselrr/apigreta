using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class externalscale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalScale_Store_StoreId",
                table: "ExternalScale");

            migrationBuilder.AddColumn<long>(
                name: "SyncDeviceId",
                table: "ExternalScale",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalScale_SyncDeviceId",
                table: "ExternalScale",
                column: "SyncDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalScale_Device_SyncDeviceId",
                table: "ExternalScale",
                column: "SyncDeviceId",
                principalTable: "Device",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalScale_Store_StoreId",
                table: "ExternalScale",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalScale_Device_SyncDeviceId",
                table: "ExternalScale");

            migrationBuilder.DropForeignKey(
                name: "FK_ExternalScale_Store_StoreId",
                table: "ExternalScale");

            migrationBuilder.DropIndex(
                name: "IX_ExternalScale_SyncDeviceId",
                table: "ExternalScale");

            migrationBuilder.DropColumn(
                name: "SyncDeviceId",
                table: "ExternalScale");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalScale_Store_StoreId",
                table: "ExternalScale",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
