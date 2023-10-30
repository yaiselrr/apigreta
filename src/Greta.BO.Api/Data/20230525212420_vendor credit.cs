using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class vendorcredit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorOrderDetailCredit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VendorOrderDetailId = table.Column<long>(type: "bigint", nullable: false),
                    CreditQuantity = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    CreditCost = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    CreditAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreditReason = table.Column<int>(type: "integer", nullable: false),
                    IsUnit = table.Column<bool>(type: "boolean", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: true),
                    ProductCode = table.Column<string>(type: "text", nullable: true),
                    ProductUpc = table.Column<string>(type: "text", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    VendorName = table.Column<string>(type: "text", nullable: true),
                    CasePack = table.Column<int>(type: "integer", nullable: false),
                    CaseCost = table.Column<decimal>(type: "numeric", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorOrderDetailCredit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorOrderDetailCredit_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorOrderDetailCredit_VendorOrderDetail_VendorOrderDetail~",
                        column: x => x.VendorOrderDetailId,
                        principalTable: "VendorOrderDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorOrderDetailCredit_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorOrderDetailCredit_ProductId",
                table: "VendorOrderDetailCredit",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorOrderDetailCredit_VendorId",
                table: "VendorOrderDetailCredit",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorOrderDetailCredit_VendorOrderDetailId",
                table: "VendorOrderDetailCredit",
                column: "VendorOrderDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorOrderDetailCredit");
        }
    }
}
