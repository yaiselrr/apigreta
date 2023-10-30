using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class adddatatomanageremployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BOUser",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 9, 39, 15, 997, DateTimeKind.Local).AddTicks(1120), new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(1230) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3120), new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3130) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3130), new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3130) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3140), new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3140) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3140), new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3140) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3150), new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3150) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3160), new DateTime(2021, 11, 6, 9, 39, 16, 0, DateTimeKind.Local).AddTicks(3160) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "BOUser");

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
    }
}
