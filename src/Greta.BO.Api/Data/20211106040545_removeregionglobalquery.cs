using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class removeregionglobalquery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 132L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 134L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 135L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 136L);

            migrationBuilder.DeleteData(
                table: "Store",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Store",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Region",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.UpdateData(
                table: "Region",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Name", "State" },
                values: new object[] { "Default", true });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 0, 5, 44, 344, DateTimeKind.Local).AddTicks(1160), new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(600) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2550), new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2560) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2560), new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2570), new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2570), new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2580), new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2590), new DateTime(2021, 11, 6, 0, 5, 44, 347, DateTimeKind.Local).AddTicks(2590) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 132L, "associate_product_image", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 22L, "Assign Image", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 134L, "associate_category_image", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, "Assign Image", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 135L, "associate_department_image", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 23L, "Assign Image", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 136L, "associate_scale_category_image", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 8L, "Assign Image", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.UpdateData(
                table: "Region",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Name", "State" },
                values: new object[] { "Unknow", false });

            migrationBuilder.InsertData(
                table: "Region",
                columns: new[] { "Id", "CreatedAt", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 2L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "East", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 25, 1, 6, 41, 234, DateTimeKind.Local).AddTicks(9990), new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(5530) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7930), new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7940) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7950), new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7950) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7950), new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7960) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7960), new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7960) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7970), new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7970) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7970), new DateTime(2021, 10, 25, 1, 6, 41, 245, DateTimeKind.Local).AddTicks(7970) });

            migrationBuilder.InsertData(
                table: "Store",
                columns: new[] { "Id", "AcceptChecksExactAmount", "AutoCloseAllCachiers", "AutoEndDate", "AutoLogOffCachiers", "CashDiscount", "CashDiscountValue", "CreatedAt", "CreditCardNeedSignature", "CreditCardNeedSignatureAmount", "Currency", "DebitCardCashBack", "DebitCardCashBackMaxAmount", "DefaulBottleDeposit", "DisplayChangeDueAfterTender", "DisplayLaneClosed", "GuidId", "Language", "LastBackupPath", "LastBackupTime", "LastBackupVersion", "MinimumAgeRequired", "Name", "PrintReceiptOptional", "RegionId", "RemotePrinters", "RoleId", "SnapEBTCAshCashBack", "SnapEBTCAshCashBackMaxAmount", "State", "SynchroVersion", "Updated", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 0m, null, false, 0m, 0m, false, false, new Guid("862f86ad-62a4-497c-bff6-360c41872524"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, "Central Park Store", false, 2L, null, null, false, 0m, true, 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 0m, null, false, 0m, 0m, false, false, new Guid("416edcff-1690-4d02-b7f4-ab65a915c621"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, "Miami Store", false, 2L, null, null, false, 0m, true, 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });
        }
    }
}
