using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class addstoretoscheduler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StoreId",
                table: "Schedule",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_StoreId",
                table: "Schedule",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Store_StoreId",
                table: "Schedule",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Store_StoreId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_StoreId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Schedule");
        }
    }
}
