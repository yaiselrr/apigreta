using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addmorefieldstosaletender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardHolderName",
                table: "SaleTender",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IssuerName",
                table: "SaleTender",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pan",
                table: "SaleTender",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardHolderName",
                table: "SaleTender");

            migrationBuilder.DropColumn(
                name: "IssuerName",
                table: "SaleTender");

            migrationBuilder.DropColumn(
                name: "Pan",
                table: "SaleTender");
        }
    }
}
