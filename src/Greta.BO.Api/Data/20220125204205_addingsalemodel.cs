using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class addingsalemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Invoice = table.Column<string>(type: "text", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    BackupComplete = table.Column<bool>(type: "boolean", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    SubTotal = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Tax = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    CashDiscount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ServiceFee = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    TenderCash = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ChangeDue = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sale_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sale_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleFee",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IncludeInItemPrice = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyFoodStamp = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyAutomatically = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyToItemQty = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyTax = table.Column<bool>(type: "boolean", nullable: false),
                    RestrictToItems = table.Column<bool>(type: "boolean", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleFee_Sale_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SaleProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UPC = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    GrossProfit = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    PLUNumber = table.Column<int>(type: "integer", nullable: true),
                    NetWeigth = table.Column<decimal>(type: "numeric(18,3)", nullable: true),
                    MixMatchDiscount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    MixAndMatchType = table.Column<int>(type: "integer", nullable: false),
                    MixAndMatchName = table.Column<string>(type: "text", nullable: true),
                    SaleDiscountId = table.Column<long>(type: "bigint", nullable: false),
                    TaxValue = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    QTY = table.Column<int>(type: "integer", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    CleanTotalPrice = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleProduct_Sale_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleTender",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ResultCode = table.Column<string>(type: "text", nullable: true),
                    ResultTxt = table.Column<string>(type: "text", nullable: true),
                    RefNum = table.Column<string>(type: "text", nullable: true),
                    RequestedAmount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    RemainingBalance = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ExtraBalance = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    BogusAccountNum = table.Column<string>(type: "text", nullable: true),
                    CardType = table.Column<string>(type: "text", nullable: true),
                    AuthCode = table.Column<string>(type: "text", nullable: true),
                    CashBack = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTender", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleTender_Sale_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleDiscount",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleProductId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ApplyToProduct = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyAutomatically = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyToCustomerOnly = table.Column<bool>(type: "boolean", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDiscount_SaleProduct_SaleProductId",
                        column: x => x.SaleProductId,
                        principalTable: "SaleProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleTax",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleId = table.Column<long>(type: "bigint", nullable: true),
                    SaleProductId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleTax_Sale_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleTax_SaleProduct_SaleProductId",
                        column: x => x.SaleProductId,
                        principalTable: "SaleProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_DeviceId",
                table: "Sale",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Invoice",
                table: "Sale",
                column: "Invoice",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sale_StoreId",
                table: "Sale",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDiscount_SaleProductId",
                table: "SaleDiscount",
                column: "SaleProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleFee_SaleId",
                table: "SaleFee",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProduct_SaleId",
                table: "SaleProduct",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleTax_SaleId",
                table: "SaleTax",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleTax_SaleProductId",
                table: "SaleTax",
                column: "SaleProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleTender_SaleId",
                table: "SaleTender",
                column: "SaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleDiscount");

            migrationBuilder.DropTable(
                name: "SaleFee");

            migrationBuilder.DropTable(
                name: "SaleTax");

            migrationBuilder.DropTable(
                name: "SaleTender");

            migrationBuilder.DropTable(
                name: "SaleProduct");

            migrationBuilder.DropTable(
                name: "Sale");
        }
    }
}
