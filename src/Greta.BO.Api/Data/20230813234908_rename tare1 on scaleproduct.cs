using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class renametare1onscaleproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tare1",
                table: "ScaleProduct",
                newName: "Tare1Old");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tare1Old",
                table: "ScaleProduct",
                newName: "Tare1");
        }
    }
}
