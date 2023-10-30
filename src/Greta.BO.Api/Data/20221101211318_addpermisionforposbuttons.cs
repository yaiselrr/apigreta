using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addpermisionforposbuttons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1014L, "posbutton_ebtbalance", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "EBT Check Balance", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1015L, "posbutton_removeservicefee", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102L, "Remove Service Fee", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1014L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1015L);
        }
    }
}
