using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class addstoretobouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeStore");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.CreateTable(
                name: "BOUserStore",
                columns: table => new
                {
                    EmployeesId = table.Column<long>(type: "bigint", nullable: false),
                    StoresId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOUserStore", x => new { x.EmployeesId, x.StoresId });
                    table.ForeignKey(
                        name: "FK_BOUserStore_BOUser_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "BOUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BOUserStore_Store_StoresId",
                        column: x => x.StoresId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOUserStore_StoresId",
                table: "BOUserStore",
                column: "StoresId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOUserStore");

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    CityName = table.Column<string>(type: "varchar(30)", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    CountryName = table.Column<string>(type: "varchar(30)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(64)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(64)", nullable: false),
                    Password = table.Column<string>(type: "varchar(6)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    ProvinceName = table.Column<string>(type: "varchar(30)", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Zip = table.Column<string>(type: "varchar(12)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeStore",
                columns: table => new
                {
                    EmployeesId = table.Column<long>(type: "bigint", nullable: false),
                    StoresId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeStore", x => new { x.EmployeesId, x.StoresId });
                    table.ForeignKey(
                        name: "FK_EmployeeStore_Employee_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeStore_Store_StoresId",
                        column: x => x.StoresId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStore_StoresId",
                table: "EmployeeStore",
                column: "StoresId");
        }
    }
}
