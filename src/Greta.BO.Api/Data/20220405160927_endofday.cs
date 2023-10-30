using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class endofday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
               name: "SaleTime",
               table: "Sale",
               type: "timestamp without time zone",
               nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EndOfDayId",
                table: "Sale",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EndOfDay",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackingType = table.Column<int>(type: "integer", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ElementId = table.Column<long>(type: "bigint", nullable: false),
                    ElementName = table.Column<string>(type: "text", nullable: true),
                    CashTotalCounted = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CashToDeposit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TenderedCashTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CashOverShort = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalNotTaxableSales = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalTaxableSales = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalSales = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalFeeAndCharges = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BottleReturnTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RefundReturn = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DebitCashBack = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    EBTCashBack = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalCash = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreditCardSales = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SnapEBTSales = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StartingCash = table.Column<int>(type: "integer", nullable: false),
                    Count100 = table.Column<int>(type: "integer", nullable: false),
                    Count50 = table.Column<int>(type: "integer", nullable: false),
                    Count20 = table.Column<int>(type: "integer", nullable: false),
                    Count10 = table.Column<int>(type: "integer", nullable: false),
                    Count5 = table.Column<int>(type: "integer", nullable: false),
                    Count1 = table.Column<int>(type: "integer", nullable: false),
                    Countc100 = table.Column<int>(type: "integer", nullable: false),
                    Countc50 = table.Column<int>(type: "integer", nullable: false),
                    Countc25 = table.Column<int>(type: "integer", nullable: false),
                    Countc10 = table.Column<int>(type: "integer", nullable: false),
                    Countc5 = table.Column<int>(type: "integer", nullable: false),
                    Countc1 = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndOfDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleTaxResume",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TaxId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    EndOfDayId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTaxResume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleTaxResume_EndOfDay_EndOfDayId",
                        column: x => x.EndOfDayId,
                        principalTable: "EndOfDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_EndOfDayId",
                table: "Sale",
                column: "EndOfDayId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleTaxResume_EndOfDayId",
                table: "SaleTaxResume",
                column: "EndOfDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_EndOfDay_EndOfDayId",
                table: "Sale",
                column: "EndOfDayId",
                principalTable: "EndOfDay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_EndOfDay_EndOfDayId",
                table: "Sale");

            migrationBuilder.DropTable(
                name: "SaleTaxResume");

            migrationBuilder.DropTable(
                name: "EndOfDay");

            migrationBuilder.DropIndex(
                name: "IX_Sale_EndOfDayId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "EndOfDayId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "SaleTime",
                table: "Sale");
        }
    }
}
