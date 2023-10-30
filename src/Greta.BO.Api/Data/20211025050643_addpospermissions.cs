using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addpospermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FunctionGroup",
                columns: new[] { "Id", "ClientApplicationId", "CreatedAt", "Name", "ProfilesId", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 102L, 2L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pos Buttons", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

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
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1002L, "posbutton_void", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Void", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1003L, "posbutton_cancel", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Cancel", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1004L, "posbutton_snap", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Snap/EBT", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1005L, "posbutton_discount", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Discount", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1006L, "posbutton_gift", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Gift Card", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1007L, "posbutton_suspend", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Suspend/Resume", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1008L, "posbutton_bottle", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Bottle Refund", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1009L, "posbutton_return", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Return", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1010L, "posbutton_zero", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Zero Scale", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1011L, "posbutton_nosale", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "No Sale", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1002L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1003L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1004L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1005L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1006L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1007L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1008L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1009L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1011L);

            migrationBuilder.DeleteData(
                table: "FunctionGroup",
                keyColumn: "Id",
                keyValue: 102L);

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 19, 18, 13, 33, 221, DateTimeKind.Local).AddTicks(6840), new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(7430) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9400), new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9410) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9410), new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9420) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9420), new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9420) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9420), new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9430) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9430), new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9430) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9440), new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9440) });
        }
    }
}
