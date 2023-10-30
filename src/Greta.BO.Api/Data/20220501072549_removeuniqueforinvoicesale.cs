using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class removeuniqueforinvoicesale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sale_Invoice",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "Applied",
                table: "Batch");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Invoice",
                table: "Sale",
                column: "Invoice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sale_Invoice",
                table: "Sale");

            migrationBuilder.AddColumn<bool>(
                name: "Applied",
                table: "Batch",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Invoice",
                table: "Sale",
                column: "Invoice",
                unique: true);
        }
    }
}
