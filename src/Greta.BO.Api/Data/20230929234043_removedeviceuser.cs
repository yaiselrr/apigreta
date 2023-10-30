using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class removedeviceuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Device_DeviceId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Sale_DeviceId",
                table: "Sale");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sale_DeviceId",
                table: "Sale",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Device_DeviceId",
                table: "Sale",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
