using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class addtraceabilitystoredata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GlobalTraceNumberGtn",
                table: "Store",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsdaEstablishNumber",
                table: "Store",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlobalTraceNumberGtn",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UsdaEstablishNumber",
                table: "Store");
        }
    }
}
