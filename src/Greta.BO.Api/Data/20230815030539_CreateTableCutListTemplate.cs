using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class CreateTableCutListTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CutListTemplateId",
                table: "CutList",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CutListTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CutListTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CutListTemplateScaleProduct",
                columns: table => new
                {
                    CutListTemplatesId = table.Column<long>(type: "bigint", nullable: false),
                    ScaleProductsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CutListTemplateScaleProduct", x => new { x.CutListTemplatesId, x.ScaleProductsId });
                    table.ForeignKey(
                        name: "FK_CutListTemplateScaleProduct_CutListTemplate_CutListTemplate~",
                        column: x => x.CutListTemplatesId,
                        principalTable: "CutListTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CutListTemplateScaleProduct_ScaleProduct_ScaleProductsId",
                        column: x => x.ScaleProductsId,
                        principalTable: "ScaleProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });            

            migrationBuilder.CreateIndex(
                name: "IX_CutList_CutListTemplateId",
                table: "CutList",
                column: "CutListTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CutListTemplate_Name",
                table: "CutListTemplate",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CutListTemplateScaleProduct_ScaleProductsId",
                table: "CutListTemplateScaleProduct",
                column: "ScaleProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CutList_CutListTemplate_CutListTemplateId",
                table: "CutList",
                column: "CutListTemplateId",
                principalTable: "CutListTemplate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CutList_CutListTemplate_CutListTemplateId",
                table: "CutList");

            migrationBuilder.DropTable(
                name: "CutListTemplateScaleProduct");

            migrationBuilder.DropTable(
                name: "CutListTemplate");

            migrationBuilder.DropIndex(
                name: "IX_CutList_CutListTemplateId",
                table: "CutList");

            migrationBuilder.DropColumn(
                name: "CutListTemplateId",
                table: "CutList");
            
        }
    }
}
