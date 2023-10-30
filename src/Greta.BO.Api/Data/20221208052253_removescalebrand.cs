using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class removescalebrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalScale_ScaleBrand_ScaleBrandId",
                table: "ExternalScale");

            migrationBuilder.DropForeignKey(
                name: "FK_ScaleLabelDefinition_ScaleBrand_ScaleBrandId",
                table: "ScaleLabelDefinition");

            migrationBuilder.DropTable(
                name: "ScaleBrand");

            migrationBuilder.DropIndex(
                name: "IX_ScaleLabelDefinition_ScaleBrandId",
                table: "ScaleLabelDefinition");

            migrationBuilder.DropIndex(
                name: "IX_ExternalScale_ScaleBrandId",
                table: "ExternalScale");

            migrationBuilder.DropColumn(
                name: "ScaleBrandId",
                table: "ExternalScale");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ScaleBrandId",
                table: "ExternalScale",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScaleBrand",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Manufacture = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleBrand", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ScaleBrand",
                columns: new[] { "Id", "CreatedAt", "Manufacture", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Greta Label", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.CreateIndex(
                name: "IX_ScaleLabelDefinition_ScaleBrandId",
                table: "ScaleLabelDefinition",
                column: "ScaleBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalScale_ScaleBrandId",
                table: "ExternalScale",
                column: "ScaleBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleBrand_Name",
                table: "ScaleBrand",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalScale_ScaleBrand_ScaleBrandId",
                table: "ExternalScale",
                column: "ScaleBrandId",
                principalTable: "ScaleBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScaleLabelDefinition_ScaleBrand_ScaleBrandId",
                table: "ScaleLabelDefinition",
                column: "ScaleBrandId",
                principalTable: "ScaleBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
