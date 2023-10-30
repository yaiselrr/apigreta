using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class cutlistunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CutList_AnimalId",
                table: "CutList");

            migrationBuilder.CreateIndex(
                name: "IX_CutList_AnimalId_CustomerId",
                table: "CutList",
                columns: new[] { "AnimalId", "CustomerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CutList_AnimalId_CustomerId",
                table: "CutList");

            migrationBuilder.CreateIndex(
                name: "IX_CutList_AnimalId",
                table: "CutList",
                column: "AnimalId");
        }
    }
}
