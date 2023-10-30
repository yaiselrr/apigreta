using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class OnlineStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlineStore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    NameWebsite = table.Column<string>(type: "varchar(100)", nullable: true),
                    LocationServerType = table.Column<int>(type: "integer", nullable: false),
                    IsActiveWebSite = table.Column<bool>(type: "boolean", nullable: false),
                    IsStockUpdated = table.Column<bool>(type: "boolean", nullable: false),
                    IsAllowStorePickup = table.Column<bool>(type: "boolean", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineStore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineStore_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineStore_Name",
                table: "OnlineStore",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineStore_NameWebsite",
                table: "OnlineStore",
                column: "NameWebsite",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineStore_StoreId",
                table: "OnlineStore",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlineStore");
        }
    }
}
