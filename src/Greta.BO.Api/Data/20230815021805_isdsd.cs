﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class isdsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDirectStoreDelivery",
                table: "VendorOrder",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Breed",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Country",
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

            migrationBuilder.UpdateData(
                table: "ExternalScale",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "LastCategoryUpdate", "LastDepartmentUpdate", "LastPluUpdate", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Rancher",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Vendor",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDirectStoreDelivery",
                table: "VendorOrder");

            migrationBuilder.UpdateData(
                table: "Breed",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Country",
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
                table: "ExternalScale",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "LastCategoryUpdate", "LastDepartmentUpdate", "LastPluUpdate", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Province",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Rancher",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Vendor",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 29, 0, 0, DateTimeKind.Utc) });
        }
    }
}
