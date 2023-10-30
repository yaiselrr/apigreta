using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class schedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RancherId = table.Column<long>(type: "bigint", nullable: true),
                    Tag = table.Column<string>(type: "text", nullable: false),
                    BreedId = table.Column<long>(type: "bigint", nullable: true),
                    DateReceived = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    DateSlaughtered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LiveWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    RailWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    SubPrimalWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CutWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_Breed_BreedId",
                        column: x => x.BreedId,
                        principalTable: "Breed",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Schedule_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Schedule_Rancher_RancherId",
                        column: x => x.RancherId,
                        principalTable: "Rancher",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "FunctionGroup",
                columns: new[] { "Id", "ClientApplicationId", "CreatedAt", "Name", "ProfilesId", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 52L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Schedule", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(    
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 165L, "view_schedule", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 52L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 166L, "add_edit_schedule", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 52L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 167L, "delete_schedule", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 52L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_BreedId",
                table: "Schedule",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_CustomerId",
                table: "Schedule",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_RancherId",
                table: "Schedule",
                column: "RancherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 165L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 166L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 167L);

            migrationBuilder.DeleteData(
                table: "FunctionGroup",
                keyColumn: "Id",
                keyValue: 52L);
        }
    }
}
