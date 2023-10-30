using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Greta.BO.Api.Data
{
    /// <inheritdoc />
    public partial class addingpricesandrenamewtare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tare",
                table: "WProduct",
                newName: "WTare");

            migrationBuilder.AddColumn<decimal>(
                name: "GrossProfit2",
                table: "StoreProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price2",
                table: "StoreProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TargetGrossProfit",
                table: "StoreProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WebGrossProfit",
                table: "StoreProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WebPrice",
                table: "StoreProduct",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsWeighted",
                table: "Product",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TimeKeeping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Begin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BeginFormatDate = table.Column<string>(type: "text", nullable: true),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndFormatDate = table.Column<string>(type: "text", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    EndDeviceId = table.Column<long>(type: "bigint", nullable: true),
                    BeginDeviceId = table.Column<long>(type: "bigint", nullable: true),
                    UserForceBeginId = table.Column<long>(type: "bigint", nullable: true),
                    UserForceBegin = table.Column<string>(type: "text", nullable: true),
                    UserForceEndId = table.Column<long>(type: "bigint", nullable: true),
                    UserForceEnd = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeKeeping", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeKeeping");

            migrationBuilder.DropColumn(
                name: "GrossProfit2",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "Price2",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "TargetGrossProfit",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "WebGrossProfit",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "WebPrice",
                table: "StoreProduct");

            migrationBuilder.DropColumn(
                name: "IsWeighted",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "WTare",
                table: "WProduct",
                newName: "Tare");
        }
    }
}
