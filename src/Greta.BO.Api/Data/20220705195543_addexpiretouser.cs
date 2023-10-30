using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addexpiretouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Expire",
                table: "BOUser",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expire",
                table: "BOUser");
        }
    }
}
