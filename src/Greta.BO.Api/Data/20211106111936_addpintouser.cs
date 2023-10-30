using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addpintouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "BOUser",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 7, 19, 34, 852, DateTimeKind.Local).AddTicks(1600), new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(5130) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7440), new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7460) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7460), new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7460) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7470), new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7470) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7470), new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7480) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7480), new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7480) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7490), new DateTime(2021, 11, 6, 7, 19, 34, 855, DateTimeKind.Local).AddTicks(7490) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pin",
                table: "BOUser");

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
    }
}
