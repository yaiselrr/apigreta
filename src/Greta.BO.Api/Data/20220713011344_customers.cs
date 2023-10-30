using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class customers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customer",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.CreateTable(
                name: "CustomerDiscount",
                columns: table => new
                {
                    CustomersId = table.Column<long>(type: "bigint", nullable: false),
                    DiscountsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDiscount", x => new { x.CustomersId, x.DiscountsId });
                    table.ForeignKey(
                        name: "FK_CustomerDiscount_Customer_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerDiscount_Discount_DiscountsId",
                        column: x => x.DiscountsId,
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerMixAndMatch",
                columns: table => new
                {
                    CustomersId = table.Column<long>(type: "bigint", nullable: false),
                    MixAndMatchesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMixAndMatch", x => new { x.CustomersId, x.MixAndMatchesId });
                    table.ForeignKey(
                        name: "FK_CustomerMixAndMatch_Customer_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerMixAndMatch_MixAndMatch_MixAndMatchesId",
                        column: x => x.MixAndMatchesId,
                        principalTable: "MixAndMatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDiscount_DiscountsId",
                table: "CustomerDiscount",
                column: "DiscountsId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerMixAndMatch_MixAndMatchesId",
                table: "CustomerMixAndMatch",
                column: "MixAndMatchesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerDiscount");

            migrationBuilder.DropTable(
                name: "CustomerMixAndMatch");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customer",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)");
        }
    }
}
