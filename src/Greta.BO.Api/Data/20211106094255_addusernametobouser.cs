using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addusernametobouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "BOUser",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 5, 42, 53, 545, DateTimeKind.Local).AddTicks(1510), new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(1520) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4240), new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4260) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4270), new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4270) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4270), new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4270) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4280), new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4280) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4290), new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4290) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4290), new DateTime(2021, 11, 6, 5, 42, 53, 549, DateTimeKind.Local).AddTicks(4290) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "BOUser");

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
    }
}
