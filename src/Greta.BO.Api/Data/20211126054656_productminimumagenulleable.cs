﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class productminimumagenulleable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ScaleLabelType",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ScaleLabelType",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ScaleLabelType",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "ScaleLabelType",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.AlterColumn<int>(
                name: "MinimumAge",
                table: "Product",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 26, 0, 46, 54, 716, DateTimeKind.Local).AddTicks(4740), new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(3620) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6210), new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6220) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6230), new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6230) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6230), new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6230) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6240), new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6240) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6240), new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6250) });

            migrationBuilder.UpdateData(
                table: "TenderType",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6250), new DateTime(2021, 11, 26, 0, 46, 54, 740, DateTimeKind.Local).AddTicks(6250) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MinimumAge",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "ScaleLabelType",
                columns: new[] { "Id", "CreatedAt", "Design", "LabelId", "Name", "ScaleType", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "{}", 0, "SHELFTAG", 0, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "{}", 0, "Greta label Example", 1, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "first External", 2, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "second External", 2, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

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
    }
}
