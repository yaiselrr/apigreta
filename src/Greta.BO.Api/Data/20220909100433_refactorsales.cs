using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class refactorsales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "DriveLicenseBirthday",
                table: "Sale",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DriveLicenseDocumentDiscriminator",
                table: "Sale",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DriveLicenseExpirationday",
                table: "Sale",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DriveLicenseFamilyName",
                table: "Sale",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriveLicenseFirstName",
                table: "Sale",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriveLicenseMiddleName",
                table: "Sale",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriveLicenseRaw",
                table: "Sale",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverLicenseNumber",
                table: "Sale",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriveLicenseBirthday",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DriveLicenseDocumentDiscriminator",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DriveLicenseExpirationday",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DriveLicenseFamilyName",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DriveLicenseFirstName",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DriveLicenseMiddleName",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DriveLicenseRaw",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "DriverLicenseNumber",
                table: "Sale");

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
                    Birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DocumentDiscriminator = table.Column<string>(type: "text", nullable: true),
                    DriverLicenseNumber = table.Column<string>(type: "text", nullable: true),
                    Expirationday = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FamilyName = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    Raw = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
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
    }
}
