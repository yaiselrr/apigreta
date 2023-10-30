using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addinfostore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Store",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Store",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slogan",
                table: "Store",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "Slogan",
                table: "Store");
        }
    }
}
