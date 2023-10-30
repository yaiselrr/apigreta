using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addingchangestoexternalscales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalScale_ScaleBrand_ScaleBrandId",
                table: "ExternalScale");

            migrationBuilder.AlterColumn<long>(
                name: "ScaleBrandId",
                table: "ExternalScale",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Port",
                table: "ExternalScale",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(64)");

            migrationBuilder.AddColumn<int>(
                name: "ExternalScaleType",
                table: "ExternalScale",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCategoryUpdate",
                table: "ExternalScale",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDepartmentUpdate",
                table: "ExternalScale",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPluUpdate",
                table: "ExternalScale",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "DepartmentExternalScale",
                columns: table => new
                {
                    DepartmentsId = table.Column<long>(type: "bigint", nullable: false),
                    ExternalScalesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentExternalScale", x => new { x.DepartmentsId, x.ExternalScalesId });
                    table.ForeignKey(
                        name: "FK_DepartmentExternalScale_Department_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentExternalScale_ExternalScale_ExternalScalesId",
                        column: x => x.ExternalScalesId,
                        principalTable: "ExternalScale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ExternalScale",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ScaleBrandId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalScale_Ip",
                table: "ExternalScale",
                column: "Ip",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentExternalScale_ExternalScalesId",
                table: "DepartmentExternalScale",
                column: "ExternalScalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalScale_ScaleBrand_ScaleBrandId",
                table: "ExternalScale",
                column: "ScaleBrandId",
                principalTable: "ScaleBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalScale_ScaleBrand_ScaleBrandId",
                table: "ExternalScale");

            migrationBuilder.DropTable(
                name: "DepartmentExternalScale");

            migrationBuilder.DropIndex(
                name: "IX_ExternalScale_Ip",
                table: "ExternalScale");

            migrationBuilder.DropColumn(
                name: "ExternalScaleType",
                table: "ExternalScale");

            migrationBuilder.DropColumn(
                name: "LastCategoryUpdate",
                table: "ExternalScale");

            migrationBuilder.DropColumn(
                name: "LastDepartmentUpdate",
                table: "ExternalScale");

            migrationBuilder.DropColumn(
                name: "LastPluUpdate",
                table: "ExternalScale");

            migrationBuilder.AlterColumn<long>(
                name: "ScaleBrandId",
                table: "ExternalScale",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Port",
                table: "ExternalScale",
                type: "varchar(64)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ExternalScale",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ScaleBrandId",
                value: 1L);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalScale_ScaleBrand_ScaleBrandId",
                table: "ExternalScale",
                column: "ScaleBrandId",
                principalTable: "ScaleBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
