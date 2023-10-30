using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class driverlicense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DriverLicenseId",
                table: "Sale",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SaleDriverLicense",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleId = table.Column<long>(type: "bigint", nullable: false),
                    Raw = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    FamilyName = table.Column<string>(type: "text", nullable: true),
                    DriverLicenseNumber = table.Column<string>(type: "text", nullable: true),
                    DocumentDiscriminator = table.Column<string>(type: "text", nullable: true),
                    Birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Expirationday = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDriverLicense", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_DriverLicenseId",
                table: "Sale",
                column: "DriverLicenseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_SaleDriverLicense_DriverLicenseId",
                table: "Sale",
                column: "DriverLicenseId",
                principalTable: "SaleDriverLicense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_SaleDriverLicense_DriverLicenseId",
                table: "Sale");

            migrationBuilder.DropTable(
                name: "SaleDriverLicense");

            migrationBuilder.DropIndex(
                name: "IX_Sale_DriverLicenseId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DriverLicenseId",
                table: "Sale");
        }
    }
}
