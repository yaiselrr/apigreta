using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class scalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scalendar",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Day = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scalendar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BreedScalendar",
                columns: table => new
                {
                    BreedsId = table.Column<long>(type: "bigint", nullable: false),
                    ScalendarsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedScalendar", x => new { x.BreedsId, x.ScalendarsId });
                    table.ForeignKey(
                        name: "FK_BreedScalendar_Breed_BreedsId",
                        column: x => x.BreedsId,
                        principalTable: "Breed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreedScalendar_Scalendar_ScalendarsId",
                        column: x => x.ScalendarsId,
                        principalTable: "Scalendar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FunctionGroup",
                columns: new[] { "Id", "ClientApplicationId", "CreatedAt", "Name", "ProfilesId", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 53L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Scalendar", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "Scalendar",
                columns: new[] { "Id", "CreatedAt", "Day", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Monday", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tuesday", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wednesday", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Thursday", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Friday", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Saturday", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sunday", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 168L, "view_scalendar", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 53L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 169L, "add_edit_scalendar", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 53L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 170L, "delete_scalendar", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 53L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BreedScalendar_ScalendarsId",
                table: "BreedScalendar",
                column: "ScalendarsId");

            migrationBuilder.CreateIndex(
                name: "IX_Scalendar_Day",
                table: "Scalendar",
                column: "Day",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreedScalendar");

            migrationBuilder.DropTable(
                name: "Scalendar");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 168L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 169L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 170L);

            migrationBuilder.DeleteData(
                table: "FunctionGroup",
                keyColumn: "Id",
                keyValue: 53L);
        }
    }
}
