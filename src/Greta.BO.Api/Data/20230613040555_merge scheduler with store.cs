using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class mergeschedulerwithstore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSchedule");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.AddColumn<long>(
                name: "StoreId",
                table: "Animal",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "AnimalCustomer",
                columns: table => new
                {
                    AnimalsId = table.Column<long>(type: "bigint", nullable: false),
                    CustomersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalCustomer", x => new { x.AnimalsId, x.CustomersId });
                    table.ForeignKey(
                        name: "FK_AnimalCustomer_Animal_AnimalsId",
                        column: x => x.AnimalsId,
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalCustomer_Customer_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animal_StoreId",
                table: "Animal",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalCustomer_CustomersId",
                table: "AnimalCustomer",
                column: "CustomersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Store_StoreId",
                table: "Animal",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Store_StoreId",
                table: "Animal");

            migrationBuilder.DropTable(
                name: "AnimalCustomer");

            migrationBuilder.DropIndex(
                name: "IX_Animal_StoreId",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Animal");

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BreedId = table.Column<long>(type: "bigint", nullable: true),
                    RancherId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CutWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    DateSlaughtered = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LiveWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    RailWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    SubPrimalWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Tag = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
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
                        name: "FK_Schedule_Rancher_RancherId",
                        column: x => x.RancherId,
                        principalTable: "Rancher",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Schedule_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerSchedule",
                columns: table => new
                {
                    CustomersId = table.Column<long>(type: "bigint", nullable: false),
                    SchedulesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSchedule", x => new { x.CustomersId, x.SchedulesId });
                    table.ForeignKey(
                        name: "FK_CustomerSchedule_Customer_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSchedule_Schedule_SchedulesId",
                        column: x => x.SchedulesId,
                        principalTable: "Schedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSchedule_SchedulesId",
                table: "CustomerSchedule",
                column: "SchedulesId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_BreedId",
                table: "Schedule",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_RancherId",
                table: "Schedule",
                column: "RancherId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_StoreId",
                table: "Schedule",
                column: "StoreId");
        }
    }
}
