using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class fixscalendarmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "Schedule");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSlaughtered",
                table: "Schedule",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayId",
                table: "Scalendar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Maxx",
                table: "Breed",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DayId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DayId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DayId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DayId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DayId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DayId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Scalendar",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DayId",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Scalendar_DayId",
                table: "Scalendar",
                column: "DayId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Scalendar_DayId",
                table: "Scalendar");

            migrationBuilder.DropColumn(
                name: "DayId",
                table: "Scalendar");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSlaughtered",
                table: "Schedule",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReceived",
                table: "Schedule",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Maxx",
                table: "Breed",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }
    }
}
