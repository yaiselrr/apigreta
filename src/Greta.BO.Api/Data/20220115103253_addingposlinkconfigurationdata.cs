using Microsoft.EntityFrameworkCore.Migrations;

namespace Greta.BO.Api.Data
{
    public partial class addingposlinkconfigurationdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayPosLinkBaudRate",
                table: "Device",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayPosLinkCommType",
                table: "Device",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayPosLinkDestIP",
                table: "Device",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayPosLinkDestPort",
                table: "Device",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayPosLinkSerialPort",
                table: "Device",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayPosLinkTimeOut",
                table: "Device",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentGatewayType",
                table: "Device",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentGatewayPosLinkBaudRate",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayPosLinkCommType",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayPosLinkDestIP",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayPosLinkDestPort",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayPosLinkSerialPort",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayPosLinkTimeOut",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayType",
                table: "Device");
        }
    }
}
