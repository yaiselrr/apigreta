using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addnewpermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FunctionGroup",
                columns: new[] { "Id", "ClientApplicationId", "CreatedAt", "Name", "ProfilesId", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 42L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "End of Day", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 140L, "close_eod", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 42L, "Close Day", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 141L, "manage_eod", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 42L, "Manage", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 140L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 141L);

            migrationBuilder.DeleteData(
                table: "FunctionGroup",
                keyColumn: "Id",
                keyValue: 42L);
        }
    }
}
