using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class fieldmaxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Maxx",
                table: "Breed",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Breed",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Maxx",
                value: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Maxx",
                table: "Breed");
        }
    }
}
