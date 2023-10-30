using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addusecustomerfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseCustomer",
                table: "Store",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseCustomer",
                table: "Store");
        }
    }
}
