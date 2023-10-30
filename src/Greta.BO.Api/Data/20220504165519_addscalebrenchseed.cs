using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addscalebrenchseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sale_Invoice",
                table: "Sale");

            migrationBuilder.DeleteData(
                table: "ScaleBrand",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.UpdateData(
                table: "ScaleBrand",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "Greta Label");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Invoice",
                table: "Sale",
                column: "Invoice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sale_Invoice",
                table: "Sale");

            migrationBuilder.UpdateData(
                table: "ScaleBrand",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "Greta LP1 Printer");

            migrationBuilder.InsertData(
                table: "ScaleBrand",
                columns: new[] { "Id", "CreatedAt", "Manufacture", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Hobart", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Invoice",
                table: "Sale",
                column: "Invoice",
                unique: true);
        }
    }
}
