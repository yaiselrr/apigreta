using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class fixproblemwithvendororder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderedDate",
                table: "VendorOrder",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "VendorOrder",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_VendorOrder_UserId",
                table: "VendorOrder",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorOrder_BOUser_UserId",
                table: "VendorOrder",
                column: "UserId",
                principalTable: "BOUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorOrder_BOUser_UserId",
                table: "VendorOrder");

            migrationBuilder.DropIndex(
                name: "IX_VendorOrder_UserId",
                table: "VendorOrder");

            migrationBuilder.DropColumn(
                name: "OrderedDate",
                table: "VendorOrder");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VendorOrder");
        }
    }
}
