using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class removestorefromsale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Store_StoreId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Sale_StoreId",
                table: "Sale");

            migrationBuilder.DeleteData(
                table: "ExternalScale",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Vendor",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.UpdateData(
                table: "Breed",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Rancher",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Breed",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "ExternalScale",
                columns: new[] { "Id", "CreatedAt", "ExternalScaleType", "Ip", "LastCategoryUpdate", "LastDepartmentUpdate", "LastPluUpdate", "Port", "State", "StoreId", "SyncDeviceId", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 0, "192.168.0.101", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "1889", true, 1L, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.UpdateData(
                table: "Rancher",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Vendor",
                columns: new[] { "Id", "AccountNumber", "Address1", "Address2", "CityName", "CountryId", "CountryName", "CreatedAt", "MinimalOrder", "Name", "Note", "ProvinceId", "ProvinceName", "State", "UpdatedAt", "UserCreatorId", "Zip" },
                values: new object[] { 1L, null, null, null, null, 0L, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 200.0, "Vendor 0", "Note", 0L, null, true, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_StoreId",
                table: "Sale",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Store_StoreId",
                table: "Sale",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
