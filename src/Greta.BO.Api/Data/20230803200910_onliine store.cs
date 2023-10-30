using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class onliinestore : Migration
    {
        /// <inheritdoc />
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
                    Type = table.Column<int>(type: "integer", nullable: false),
                    LocationServerType = table.Column<int>(type: "integer", nullable: false),
                    IsActiveWebSite = table.Column<bool>(type: "boolean", nullable: false),
                    IsStockUpdated = table.Column<bool>(type: "boolean", nullable: false),
                    IsAllowStorePickup = table.Column<bool>(type: "boolean", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "DepartmentOnlineStore",
                columns: table => new
                {
                    DepartmentsId = table.Column<long>(type: "bigint", nullable: false),
                    OnlineStoresId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentOnlineStore", x => new { x.DepartmentsId, x.OnlineStoresId });
                    table.ForeignKey(
                        name: "FK_DepartmentOnlineStore_Department_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentOnlineStore_OnlineStore_OnlineStoresId",
                        column: x => x.OnlineStoresId,
                        principalTable: "OnlineStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnlineCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnlineStoreId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    OnlineCategoryId = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnlineCategory_OnlineStore_OnlineStoreId",
                        column: x => x.OnlineStoreId,
                        principalTable: "OnlineStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnlineProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnlineStoreId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    OnlineProductId = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineProduct_OnlineStore_OnlineStoreId",
                        column: x => x.OnlineStoreId,
                        principalTable: "OnlineStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnlineProduct_StoreProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "StoreProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOnlineStore_OnlineStoresId",
                table: "DepartmentOnlineStore",
                column: "OnlineStoresId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineCategory_CategoryId",
                table: "OnlineCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineCategory_OnlineStoreId",
                table: "OnlineCategory",
                column: "OnlineStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineProduct_OnlineStoreId",
                table: "OnlineProduct",
                column: "OnlineStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineProduct_ProductId",
                table: "OnlineProduct",
                column: "ProductId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentOnlineStore");

            migrationBuilder.DropTable(
                name: "OnlineCategory");

            migrationBuilder.DropTable(
                name: "OnlineProduct");

            migrationBuilder.DropTable(
                name: "OnlineStore");
        }
    }
}
