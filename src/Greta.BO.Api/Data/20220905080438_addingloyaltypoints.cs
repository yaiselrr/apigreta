using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class addingloyaltypoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoyaltyDiscount",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Maximum = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoyaltyDiscount_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FunctionGroup",
                columns: new[] { "Id", "ClientApplicationId", "CreatedAt", "Name", "ProfilesId", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 45L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Loyalty Discount", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 144L, "view_loyalty_discount", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 45L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 145L, "add_edit_loyalty_discount", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 45L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 146L, "delete_loyalty_discount", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 45L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyDiscount_StoreId",
                table: "LoyaltyDiscount",
                column: "StoreId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoyaltyDiscount");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 144L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 145L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 146L);

            migrationBuilder.DeleteData(
                table: "FunctionGroup",
                keyColumn: "Id",
                keyValue: 45L);
        }
    }
}
