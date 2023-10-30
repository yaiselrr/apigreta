using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class addingbatchclosedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchClose",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    AcquirerName = table.Column<string>(type: "text", nullable: true),
                    EBTAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Batch = table.Column<string>(type: "text", nullable: true),
                    BatchRecordCount = table.Column<int>(type: "integer", nullable: false),
                    HostResponseText = table.Column<string>(type: "text", nullable: true),
                    HostTotalsAmount1 = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    HostTotalsAmount5 = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    HostTotalsCount1 = table.Column<int>(type: "integer", nullable: false),
                    HostTotalsCount5 = table.Column<int>(type: "integer", nullable: false),
                    MerchantName = table.Column<string>(type: "text", nullable: true),
                    TerminalNumber = table.Column<string>(type: "text", nullable: true),
                    TransactionResponse = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchClose", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchClose_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchClose_DeviceId",
                table: "BatchClose",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchClose");
        }
    }
}
