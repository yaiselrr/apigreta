using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class Adddiscountonlyapply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotAllowAnyOtherDiscount",
                table: "MixAndMatch",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotAllowAnyOtherDiscount",
                table: "Discount",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotAllowAnyOtherDiscount",
                table: "MixAndMatch");

            migrationBuilder.DropColumn(
                name: "NotAllowAnyOtherDiscount",
                table: "Discount");
        }
    }
}
