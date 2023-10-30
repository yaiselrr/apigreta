﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class addfielddesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScaleLabelDesign");

            migrationBuilder.AddColumn<string>(
                name: "Design",
                table: "ScaleLabelType",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Design",
                table: "ScaleLabelType");

            migrationBuilder.CreateTable(
                name: "ScaleLabelDesign",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Design = table.Column<string>(type: "text", nullable: true),
                    ScaleLabelTypeId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleLabelDesign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScaleLabelDesign_ScaleLabelType_ScaleLabelTypeId",
                        column: x => x.ScaleLabelTypeId,
                        principalTable: "ScaleLabelType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScaleLabelDesign_ScaleLabelTypeId",
                table: "ScaleLabelDesign",
                column: "ScaleLabelTypeId",
                unique: true);
        }
    }
}
