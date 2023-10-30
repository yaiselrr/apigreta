using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class reasoncodepermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FunctionGroup",
                columns: new[] { "Id", "ClientApplicationId", "CreatedAt", "Name", "ProfilesId", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 47L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reason Codes", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 150L, "view_reason_codes", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 47L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 151L, "add_edit_reason_codes", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 47L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 152L, "delete_reason_codes", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 47L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 150L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 151L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 152L);

            migrationBuilder.DeleteData(
                table: "FunctionGroup",
                keyColumn: "Id",
                keyValue: 47L);
        }
    }
}
