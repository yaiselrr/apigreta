using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class AddSaleRemovedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaleProductRemoved",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: true),
                    UPC = table.Column<string>(type: "text", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    EmployeeAcceptId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeAcceptName = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleProductRemoved", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleProductRemoved_Sale_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 1017L, "allow_remove_product", new DateTime(2021, 3, 10, 5, 0, 0, 0, DateTimeKind.Utc), 101L, "Remove product from sale", true, new DateTime(2021, 3, 10, 5, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.CreateIndex(
                name: "IX_SaleProductRemoved_SaleId",
                table: "SaleProductRemoved",
                column: "SaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleProductRemoved");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1017L);
        }
    }
}
