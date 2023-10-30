using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class delcustomersch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Customer_CustomerId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_CustomerId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Schedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "Schedule",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_CustomerId",
                table: "Schedule",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Customer_CustomerId",
                table: "Schedule",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
