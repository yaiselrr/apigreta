using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class timekeeping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TimeWorked",
                table: "TimeKeeping",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AddColumn<long>(
                name: "BeginStoreId",
                table: "TimeKeeping",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeginStoreName",
                table: "TimeKeeping",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EndStoreId",
                table: "TimeKeeping",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndStoreName",
                table: "TimeKeeping",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginStoreId",
                table: "TimeKeeping");

            migrationBuilder.DropColumn(
                name: "BeginStoreName",
                table: "TimeKeeping");

            migrationBuilder.DropColumn(
                name: "EndStoreId",
                table: "TimeKeeping");

            migrationBuilder.DropColumn(
                name: "EndStoreName",
                table: "TimeKeeping");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TimeWorked",
                table: "TimeKeeping",
                type: "interval",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
