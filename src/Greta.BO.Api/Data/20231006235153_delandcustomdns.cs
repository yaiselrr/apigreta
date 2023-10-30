using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class delandcustomdns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomDns",
                table: "OnlineStore",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Isdeleted",
                table: "OnlineStore",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomDns",
                table: "OnlineStore");

            migrationBuilder.DropColumn(
                name: "Isdeleted",
                table: "OnlineStore");
        }
    }
}
