using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class removecascadefromsaleforingkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_EndOfDay_EndOfDayId",
                table: "Sale");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_EndOfDay_EndOfDayId",
                table: "Sale",
                column: "EndOfDayId",
                principalTable: "EndOfDay",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_EndOfDay_EndOfDayId",
                table: "Sale");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_EndOfDay_EndOfDayId",
                table: "Sale",
                column: "EndOfDayId",
                principalTable: "EndOfDay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
