using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Greta.BO.Api.Data
{
    public partial class migratetopostgres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientApplication",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(40)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientApplication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(40)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CSVMapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MapperJson = table.Column<string>(type: "text", nullable: false),
                    ModelImport = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSVMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    Perishable = table.Column<bool>(type: "boolean", nullable: false),
                    BackgroundColor = table.Column<string>(type: "text", nullable: true),
                    ForegroundColor = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Family",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Family", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fee",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(40)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IncludeInItemPrice = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyFoodStamp = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyAutomatically = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyToItemQty = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyTax = table.Column<bool>(type: "boolean", nullable: false),
                    RestrictToItems = table.Column<bool>(type: "boolean", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Path = table.Column<string>(type: "text", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GuidId = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScaleBrand",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    Manufacture = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleBrand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScaleLabelType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    LabelId = table.Column<int>(type: "integer", nullable: false),
                    ScaleType = table.Column<int>(type: "integer", nullable: false),
                    Design = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleLabelType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tax",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    Description = table.Column<string>(type: "varchar(254)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tax", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenderType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    OpenDrawer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DisplayAs = table.Column<string>(type: "varchar(64)", nullable: false),
                    CashDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    GuidId = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    AccountNumber = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "varchar(254)", nullable: true),
                    MinimalOrder = table.Column<decimal>(type: "numeric(15,3)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    CityName = table.Column<string>(type: "varchar(30)", nullable: true),
                    ProvinceName = table.Column<string>(type: "varchar(30)", nullable: true),
                    CountryName = table.Column<string>(type: "varchar(30)", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Zip = table.Column<string>(type: "varchar(12)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_ClientApplication_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "ClientApplication",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(40)", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Province_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScaleCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    BackgroundColor = table.Column<string>(type: "text", nullable: true),
                    ForegroundColor = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScaleCategory_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FamilyFee",
                columns: table => new
                {
                    FamiliesId = table.Column<long>(type: "bigint", nullable: false),
                    FeesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyFee", x => new { x.FamiliesId, x.FeesId });
                    table.ForeignKey(
                        name: "FK_FamilyFee_Family_FamiliesId",
                        column: x => x.FamiliesId,
                        principalTable: "Family",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyFee_Fee_FeesId",
                        column: x => x.FeesId,
                        principalTable: "Fee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    AllStores = table.Column<bool>(type: "boolean", nullable: false),
                    RegionId = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    Description = table.Column<string>(type: "varchar(254)", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    DefaulShelfTagId = table.Column<long>(type: "bigint", nullable: true),
                    PromptPriceAtPOS = table.Column<bool>(type: "boolean", nullable: false),
                    SnapEBT = table.Column<bool>(type: "boolean", nullable: false),
                    PrintShelfTag = table.Column<bool>(type: "boolean", nullable: false),
                    NoPriceOnShelfTag = table.Column<bool>(type: "boolean", nullable: false),
                    AllowZeroStock = table.Column<bool>(type: "boolean", nullable: false),
                    MinimumAge = table.Column<int>(type: "integer", nullable: true),
                    NoDiscountAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    AddOnlineStore = table.Column<bool>(type: "boolean", nullable: false),
                    Modifier = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayStockOnPosButton = table.Column<bool>(type: "boolean", nullable: false),
                    BackgroundColor = table.Column<string>(type: "text", nullable: true),
                    ForegroundColor = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Category_ScaleLabelType_DefaulShelfTagId",
                        column: x => x.DefaulShelfTagId,
                        principalTable: "ScaleLabelType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VendorContact",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Contact = table.Column<string>(type: "varchar(64)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(14)", nullable: false),
                    Primary = table.Column<bool>(type: "boolean", nullable: false),
                    Fax = table.Column<string>(type: "varchar(60)", nullable: true),
                    Email = table.Column<string>(type: "varchar(60)", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorContact_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FunctionGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(40)", nullable: false),
                    ClientApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    ProfilesId = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionGroup_ClientApplication_ClientApplicationId",
                        column: x => x.ClientApplicationId,
                        principalTable: "ClientApplication",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FunctionGroup_Profiles_ProfilesId",
                        column: x => x.ProfilesId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(40)", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    BOProfileId = table.Column<long>(type: "bigint", nullable: true),
                    POSProfileId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BOUser_Profiles_BOProfileId",
                        column: x => x.BOProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BOUser_Profiles_POSProfileId",
                        column: x => x.POSProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BOUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    RegionId = table.Column<long>(type: "bigint", nullable: false),
                    Updated = table.Column<bool>(type: "boolean", nullable: false),
                    LastBackupTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastBackupPath = table.Column<string>(type: "text", nullable: true),
                    LastBackupVersion = table.Column<int>(type: "integer", nullable: false),
                    SynchroVersion = table.Column<int>(type: "integer", nullable: false),
                    RemotePrinters = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    CashDiscount = table.Column<bool>(type: "boolean", nullable: false),
                    CashDiscountValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AcceptChecksExactAmount = table.Column<bool>(type: "boolean", nullable: false),
                    CreditCardNeedSignature = table.Column<bool>(type: "boolean", nullable: false),
                    CreditCardNeedSignatureAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DebitCardCashBack = table.Column<bool>(type: "boolean", nullable: false),
                    DebitCardCashBackMaxAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SnapEBTCAshCashBack = table.Column<bool>(type: "boolean", nullable: false),
                    SnapEBTCAshCashBackMaxAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MinimumAgeRequired = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayChangeDueAfterTender = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayLaneClosed = table.Column<bool>(type: "boolean", nullable: false),
                    DefaulBottleDeposit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PrintReceiptOptional = table.Column<bool>(type: "boolean", nullable: false),
                    AutoLogOffCachiers = table.Column<int>(type: "integer", nullable: false),
                    AutoEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AutoCloseAllCachiers = table.Column<bool>(type: "boolean", nullable: false),
                    GuidId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Store_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Store_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryFee",
                columns: table => new
                {
                    CategoriesId = table.Column<long>(type: "bigint", nullable: false),
                    FeesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryFee", x => new { x.CategoriesId, x.FeesId });
                    table.ForeignKey(
                        name: "FK_CategoryFee_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryFee_Fee_FeesId",
                        column: x => x.FeesId,
                        principalTable: "Fee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTax",
                columns: table => new
                {
                    CategoriesId = table.Column<long>(type: "bigint", nullable: false),
                    TaxsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTax", x => new { x.CategoriesId, x.TaxsId });
                    table.ForeignKey(
                        name: "FK_CategoryTax_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTax_Tax_TaxsId",
                        column: x => x.TaxsId,
                        principalTable: "Tax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ApplyToProduct = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyAutomatically = table.Column<bool>(type: "boolean", nullable: false),
                    ApplyToCustomerOnly = table.Column<bool>(type: "boolean", nullable: false),
                    ActiveOnPeriod = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Monday = table.Column<bool>(type: "boolean", nullable: true),
                    Tuesday = table.Column<bool>(type: "boolean", nullable: true),
                    Wednesday = table.Column<bool>(type: "boolean", nullable: true),
                    Thursday = table.Column<bool>(type: "boolean", nullable: true),
                    Friday = table.Column<bool>(type: "boolean", nullable: true),
                    Saturday = table.Column<bool>(type: "boolean", nullable: true),
                    Sunday = table.Column<bool>(type: "boolean", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    FamilyId = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discount_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Discount_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Discount_Family_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Family",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UPC = table.Column<string>(type: "varchar(40)", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    FamilyId = table.Column<long>(type: "bigint", nullable: true),
                    DefaulShelfTagId = table.Column<long>(type: "bigint", nullable: true),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    MinimumAge = table.Column<int>(type: "integer", nullable: false),
                    PosVisible = table.Column<bool>(type: "boolean", nullable: false),
                    ScaleVisible = table.Column<bool>(type: "boolean", nullable: false),
                    AllowZeroStock = table.Column<bool>(type: "boolean", nullable: false),
                    NoDiscountAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    PromptPriceAtPOS = table.Column<bool>(type: "boolean", nullable: false),
                    SnapEBT = table.Column<bool>(type: "boolean", nullable: false),
                    PrintShelfTag = table.Column<bool>(type: "boolean", nullable: false),
                    NoPriceOnShelfTag = table.Column<bool>(type: "boolean", nullable: false),
                    AddOnlineStore = table.Column<bool>(type: "boolean", nullable: false),
                    Modifier = table.Column<bool>(type: "boolean", nullable: false),
                    LoyaltyPoints = table.Column<int>(type: "integer", nullable: false),
                    DisplayStockOnPosButton = table.Column<bool>(type: "boolean", nullable: false),
                    QtyHand = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OrderTrigger = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OrderAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_Family_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Family",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_ScaleLabelType_DefaulShelfTagId",
                        column: x => x.DefaulShelfTagId,
                        principalTable: "ScaleLabelType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(40)", nullable: false),
                    Code = table.Column<string>(type: "varchar(40)", nullable: false),
                    FunctionGroupId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_FunctionGroup_FunctionGroupId",
                        column: x => x.FunctionGroupId,
                        principalTable: "FunctionGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastName = table.Column<string>(type: "varchar(64)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(64)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    TaxExcept = table.Column<bool>(type: "boolean", nullable: false),
                    StoreCredit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    CityName = table.Column<string>(type: "varchar(30)", nullable: true),
                    ProvinceName = table.Column<string>(type: "varchar(30)", nullable: true),
                    CountryName = table.Column<string>(type: "varchar(30)", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Zip = table.Column<string>(type: "varchar(12)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastName = table.Column<string>(type: "varchar(64)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(64)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    Password = table.Column<string>(type: "varchar(6)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    CityName = table.Column<string>(type: "varchar(30)", nullable: true),
                    ProvinceName = table.Column<string>(type: "varchar(30)", nullable: true),
                    CountryName = table.Column<string>(type: "varchar(30)", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Zip = table.Column<string>(type: "varchar(12)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Batch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batch_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DeviceId = table.Column<string>(type: "text", nullable: true),
                    GuidId = table.Column<Guid>(type: "uuid", nullable: false),
                    SynchroVersion = table.Column<int>(type: "integer", nullable: false),
                    SignalRConnectionId = table.Column<string>(type: "text", nullable: true),
                    LastPongTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    PrinterType = table.Column<int>(type: "integer", nullable: false),
                    PrinterName = table.Column<string>(type: "text", nullable: true),
                    LabelPrinterType = table.Column<int>(type: "integer", nullable: false),
                    LabelPrinterName = table.Column<string>(type: "text", nullable: true),
                    Printers = table.Column<string>(type: "text", nullable: true),
                    ScaleModel = table.Column<int>(type: "integer", nullable: false),
                    ScaleComName = table.Column<string>(type: "text", nullable: true),
                    ScaleBaudRate = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Device_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalScale",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ip = table.Column<string>(type: "varchar(64)", nullable: false),
                    Port = table.Column<string>(type: "varchar(64)", nullable: false),
                    ScaleBrandId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalScale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalScale_ScaleBrand_ScaleBrandId",
                        column: x => x.ScaleBrandId,
                        principalTable: "ScaleBrand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalScale_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScaleHomeFav",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleHomeFav", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScaleHomeFav_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScaleHomeFav_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreTax",
                columns: table => new
                {
                    StoresId = table.Column<long>(type: "bigint", nullable: false),
                    TaxsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTax", x => new { x.StoresId, x.TaxsId });
                    table.ForeignKey(
                        name: "FK_StoreTax_Store_StoresId",
                        column: x => x.StoresId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreTax_Tax_TaxsId",
                        column: x => x.TaxsId,
                        principalTable: "Tax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Synchro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tag = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synchro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Synchro_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountProduct",
                columns: table => new
                {
                    DiscountsId = table.Column<long>(type: "bigint", nullable: false),
                    ProductsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountProduct", x => new { x.DiscountsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_DiscountProduct_Discount_DiscountsId",
                        column: x => x.DiscountsId,
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountProduct_Product_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeProduct",
                columns: table => new
                {
                    FeesId = table.Column<long>(type: "bigint", nullable: false),
                    ProductsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeProduct", x => new { x.FeesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_FeeProduct_Fee_FeesId",
                        column: x => x.FeesId,
                        principalTable: "Fee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeProduct_Product_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KitProduct_Product_Id",
                        column: x => x.Id,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixAndMatch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    QTY = table.Column<int>(type: "integer", nullable: false),
                    MixAndMatchType = table.Column<int>(type: "integer", nullable: false),
                    ActivePeriod = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ProductBuyId = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixAndMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixAndMatch_Product_ProductBuyId",
                        column: x => x.ProductBuyId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScaleProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PLUNumber = table.Column<int>(type: "integer", nullable: false),
                    ScaleCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    PLUType = table.Column<int>(type: "integer", nullable: false),
                    Description1 = table.Column<string>(type: "varchar(300)", nullable: false),
                    Description2 = table.Column<string>(type: "varchar(300)", nullable: true),
                    Description3 = table.Column<string>(type: "varchar(300)", nullable: true),
                    ByCount = table.Column<int>(type: "integer", nullable: false),
                    ShelfLife = table.Column<int>(type: "integer", nullable: false),
                    ProductLife = table.Column<int>(type: "integer", nullable: false),
                    PackageWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Text1 = table.Column<string>(type: "varchar(1000)", nullable: true),
                    Text2 = table.Column<string>(type: "varchar(1000)", nullable: true),
                    Text3 = table.Column<string>(type: "varchar(1000)", nullable: true),
                    Text4 = table.Column<string>(type: "varchar(1000)", nullable: true),
                    Tare1 = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Tare2 = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ForceTare = table.Column<bool>(type: "boolean", nullable: false),
                    TareIsPercent = table.Column<bool>(type: "boolean", nullable: false),
                    ServingPerContainer = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ServingSize = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AmountPerServingCalories = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalFatGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalFat = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SaturedFatGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SaturedFat = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CholesterolMGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Cholesterol = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SodiumMGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Sodium = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalCarbohydrateGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalCarbohydrate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DietaryFiberGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DietaryFiber = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalSugarGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AddedSugarGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AddedSugar = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ProteinGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    VitDMGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    VitD = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CalciumMGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Calcium = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IronMGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Iron = table.Column<double>(type: "double precision", nullable: false),
                    PotasMGrams = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Potas = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScaleProduct_Product_Id",
                        column: x => x.Id,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScaleProduct_ScaleCategory_ScaleCategoryId",
                        column: x => x.ScaleCategoryId,
                        principalTable: "ScaleCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BinLocation = table.Column<string>(type: "text", nullable: true),
                    GrossProfit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreProduct_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductCode = table.Column<string>(type: "text", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CasePack = table.Column<int>(type: "integer", nullable: false),
                    CaseCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OrderByCase = table.Column<int>(type: "integer", nullable: false),
                    LastOrderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorProduct_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tare = table.Column<decimal>(type: "numeric(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WProduct_Product_Id",
                        column: x => x.Id,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionProfiles",
                columns: table => new
                {
                    PermissionsId = table.Column<long>(type: "bigint", nullable: false),
                    ProfilesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionProfiles", x => new { x.PermissionsId, x.ProfilesId });
                    table.ForeignKey(
                        name: "FK_PermissionProfiles_Permission_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionProfiles_Profiles_ProfilesId",
                        column: x => x.ProfilesId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "PriceBatchDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    FamilyId = table.Column<long>(type: "bigint", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceBatchDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceBatchDetail_Batch_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceBatchDetail_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PriceBatchDetail_Family_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Family",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PriceBatchDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SynchroDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Entity = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: true),
                    SynchroId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SynchroDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SynchroDetail_Synchro_SynchroId",
                        column: x => x.SynchroId,
                        principalTable: "Synchro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitProductProduct",
                columns: table => new
                {
                    KitProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitProductProduct", x => new { x.ProductId, x.KitProductId });
                    table.ForeignKey(
                        name: "FK_KitProductProduct_KitProduct_KitProductId",
                        column: x => x.KitProductId,
                        principalTable: "KitProduct",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KitProductProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FamilyMixAndMatch",
                columns: table => new
                {
                    FamiliesId = table.Column<long>(type: "bigint", nullable: false),
                    MixAndMatchsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMixAndMatch", x => new { x.FamiliesId, x.MixAndMatchsId });
                    table.ForeignKey(
                        name: "FK_FamilyMixAndMatch_Family_FamiliesId",
                        column: x => x.FamiliesId,
                        principalTable: "Family",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyMixAndMatch_MixAndMatch_MixAndMatchsId",
                        column: x => x.MixAndMatchsId,
                        principalTable: "MixAndMatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixAndMatchProduct",
                columns: table => new
                {
                    MixAndMatchsId = table.Column<long>(type: "bigint", nullable: false),
                    ProductsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixAndMatchProduct", x => new { x.MixAndMatchsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_MixAndMatchProduct_MixAndMatch_MixAndMatchsId",
                        column: x => x.MixAndMatchsId,
                        principalTable: "MixAndMatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixAndMatchProduct_Product_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScaleHomeFavScaleProduct",
                columns: table => new
                {
                    ScaleHomeFavsId = table.Column<long>(type: "bigint", nullable: false),
                    ScaleProductsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleHomeFavScaleProduct", x => new { x.ScaleHomeFavsId, x.ScaleProductsId });
                    table.ForeignKey(
                        name: "FK_ScaleHomeFavScaleProduct_ScaleHomeFav_ScaleHomeFavsId",
                        column: x => x.ScaleHomeFavsId,
                        principalTable: "ScaleHomeFav",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScaleHomeFavScaleProduct_ScaleProduct_ScaleProductsId",
                        column: x => x.ScaleProductsId,
                        principalTable: "ScaleProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScaleLabelDefinition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScaleProductId = table.Column<long>(type: "bigint", nullable: false),
                    ScaleLabelType1Id = table.Column<long>(type: "bigint", nullable: false),
                    ScaleLabelType2Id = table.Column<long>(type: "bigint", nullable: false),
                    ScaleBrandId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleLabelDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScaleLabelDefinition_ScaleBrand_ScaleBrandId",
                        column: x => x.ScaleBrandId,
                        principalTable: "ScaleBrand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScaleLabelDefinition_ScaleLabelType_ScaleLabelType1Id",
                        column: x => x.ScaleLabelType1Id,
                        principalTable: "ScaleLabelType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScaleLabelDefinition_ScaleLabelType_ScaleLabelType2Id",
                        column: x => x.ScaleLabelType2Id,
                        principalTable: "ScaleLabelType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScaleLabelDefinition_ScaleProduct_ScaleProductId",
                        column: x => x.ScaleProductId,
                        principalTable: "ScaleProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreProductTax",
                columns: table => new
                {
                    StoreProductsId = table.Column<long>(type: "bigint", nullable: false),
                    TaxsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreProductTax", x => new { x.StoreProductsId, x.TaxsId });
                    table.ForeignKey(
                        name: "FK_StoreProductTax_StoreProduct_StoreProductsId",
                        column: x => x.StoreProductsId,
                        principalTable: "StoreProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreProductTax_Tax_TaxsId",
                        column: x => x.TaxsId,
                        principalTable: "Tax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClientApplication",
                columns: new[] { "Id", "CreatedAt", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Back Office", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "POS", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "CreatedAt", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "United State", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "BackgroundColor", "CreatedAt", "DepartmentId", "ForegroundColor", "Name", "Perishable", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 1L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Default", false, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "Family",
                columns: new[] { "Id", "CreatedAt", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family 0", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family 1", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family 2", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family 3", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family 4", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "Region",
                columns: new[] { "Id", "CreatedAt", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unknow", false, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "East", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "AllStores", "CreatedAt", "Name", "RegionId", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 1L, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrator", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "ScaleBrand",
                columns: new[] { "Id", "CreatedAt", "Manufacture", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Greta LP1 Printer", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Hobart", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "ScaleLabelType",
                columns: new[] { "Id", "CreatedAt", "Design", "LabelId", "Name", "ScaleType", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "second External", 2, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "first External", 2, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "{}", 0, "SHELFTAG", 0, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "{}", 0, "Greta label Example", 1, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "TenderType",
                columns: new[] { "Id", "CashDiscount", "CreatedAt", "DisplayAs", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 7L, false, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9440), "Gift Card", "Gift Card", true, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9440), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "TenderType",
                columns: new[] { "Id", "CashDiscount", "CreatedAt", "DisplayAs", "Name", "OpenDrawer", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 1L, true, new DateTime(2021, 10, 19, 18, 13, 33, 221, DateTimeKind.Local).AddTicks(6840), "Cash", "Cash", true, true, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(7430), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "TenderType",
                columns: new[] { "Id", "CashDiscount", "CreatedAt", "DisplayAs", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 2L, false, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9400), "Check", "Check", true, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9410), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 3L, false, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9410), "Credit Card", "Credit Card", true, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9420), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 4L, false, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9420), "Debit Card", "Debit Card", true, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9420), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 5L, false, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9420), "Snap/EBT", "Snap/EBT", true, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9430), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 6L, false, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9430), "Snap/EBT Cash", "Snap/EBT Cash", true, new DateTime(2021, 10, 19, 18, 13, 33, 224, DateTimeKind.Local).AddTicks(9430), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "Vendor",
                columns: new[] { "Id", "AccountNumber", "Address1", "Address2", "CityId", "CityName", "CountryId", "CountryName", "CreatedAt", "MinimalOrder", "Name", "Note", "ProvinceId", "ProvinceName", "State", "UpdatedAt", "UserCreatorId", "Zip" },
                values: new object[] { 1L, null, null, null, 0L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 200m, "Vendor 0", "Note", 0L, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null });

            migrationBuilder.InsertData(
                table: "FunctionGroup",
                columns: new[] { "Id", "ClientApplicationId", "CreatedAt", "Name", "ProfilesId", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tax", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 101L, 2L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sell", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 41L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Device", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 40L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fees and Charges ", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 37L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scale Home FAV", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 33L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Home Screen", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 31L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "POS Config", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 30L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General Setting", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 29L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 28L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 27L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Report", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 26L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Region", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 25L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Price Family", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 24L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shelf Tags", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 22L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 21L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mix and Match", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 23L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Department", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 19L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Price Batch", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Discount", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 3L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 4L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "External Scale", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 5L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 6L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Profiles", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 20L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ad Batch", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 8L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scale Category", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 7L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scale Brand", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 10L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 12L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tender Type", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 13L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vendor", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 14L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vendor Contact", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 17L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 18L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Role", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 9L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scale label type", null, true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "ApplicationId", "CreatedAt", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrator", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, 2L, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "Id", "CountryId", "CreatedAt", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Florida", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Texas", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "Store",
                columns: new[] { "Id", "AcceptChecksExactAmount", "AutoCloseAllCachiers", "AutoEndDate", "AutoLogOffCachiers", "CashDiscount", "CashDiscountValue", "CreatedAt", "CreditCardNeedSignature", "CreditCardNeedSignatureAmount", "Currency", "DebitCardCashBack", "DebitCardCashBackMaxAmount", "DefaulBottleDeposit", "DisplayChangeDueAfterTender", "DisplayLaneClosed", "GuidId", "Language", "LastBackupPath", "LastBackupTime", "LastBackupVersion", "MinimumAgeRequired", "Name", "PrintReceiptOptional", "RegionId", "RemotePrinters", "RoleId", "SnapEBTCAshCashBack", "SnapEBTCAshCashBackMaxAmount", "State", "SynchroVersion", "Updated", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 1L, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 0m, null, false, 0m, 0m, false, false, new Guid("862f86ad-62a4-497c-bff6-360c41872524"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, "Central Park Store", false, 2L, null, null, false, 0m, true, 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 0m, null, false, 0m, 0m, false, false, new Guid("416edcff-1690-4d02-b7f4-ab65a915c621"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, "Miami Store", false, 2L, null, null, false, 0m, true, 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "Id", "CreatedAt", "Name", "ProvinceId", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dallas", 2L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Miami", 1L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "ExternalScale",
                columns: new[] { "Id", "CreatedAt", "Ip", "Port", "ScaleBrandId", "State", "StoreId", "UpdatedAt", "UserCreatorId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "192.168.0.101", "1889", 1L, true, 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "FunctionGroupId", "Name", "State", "UpdatedAt", "UserCreatorId" },
                values: new object[,]
                {
                    { 138L, "view_device", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 41L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 76L, "view_region", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 26L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 75L, "delete_price_family", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 25L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 74L, "add_edit_price_family", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 25L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 73L, "view_price_family", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 25L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 72L, "delete_shelf_tags", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 24L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 71L, "add_edit_shelf_tags", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 24L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 70L, "view_shelf_tags", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 24L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 135L, "associate_department_image", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 23L, "Assign Image", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 69L, "delete_department", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 23L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 68L, "add_edit_department", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 23L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 67L, "view_department", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 23L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 133L, "associate_product_scale_label_definition", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 22L, "Assign Scale Label Definitiion", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 132L, "associate_product_image", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 22L, "Assign Image", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 131L, "associate_product_vendor", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 22L, "Assign Vendor", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 130L, "associate_product_store", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 22L, "Assign Store", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 66L, "delete_product", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 22L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 65L, "add_edit_product", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 22L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 64L, "view_product", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 22L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 63L, "delete_mix_and_match", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 21L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 62L, "add_edit_mix_and_match", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 21L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 61L, "view_mix_and_match", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 21L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 77L, "add_edit_region", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 26L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1001L, "allow_sell", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 101L, "Sell", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 78L, "delete_region", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 26L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 60L, "delete_ad_batch", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 20L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 137L, "add_edit_device", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 41L, "Add / Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 120L, "delete_fee", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 40L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 119L, "add_edit_fee", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 40L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 118L, "view_fee", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 40L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 111L, "delete_scale_home_fav", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 37L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 110L, "add_edit_scale_home_fav", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 37L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 109L, "view_scale_home_fav", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 37L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 99L, "delete_home_screen", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 33L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 98L, "add_edit_home_screen", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 33L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 97L, "view_home_screen", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 33L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 93L, "delete_pos_config", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 31L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 92L, "add_edit_pos_config", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 31L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 91L, "view_pos_config", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 31L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 90L, "delete_general_setting", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 30L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 89L, "add_edit_general_setting", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 30L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 88L, "view_general_setting", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 30L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 87L, "delete_customer", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 29L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 86L, "add_edit_customer", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 29L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 85L, "view_customer", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 29L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 84L, "delete_employee", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 28L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 83L, "add_edit_employee", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 28L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 139L, "view_report", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 27L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 82L, "view_employee", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 28L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 59L, "add_edit_ad_batch", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 20L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 57L, "delete_price_batch", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 19L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 22L, "view_scale_category", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 8L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 21L, "delete_scale_brand", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 7L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 20L, "add_edit_scale_brand", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 7L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 19L, "view_scale_brand", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 7L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 18L, "delete_profiles", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 6L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 17L, "add_edit_profiles", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 6L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 16L, "view_profiles", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 6L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 15L, "delete_family", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 14L, "add_edit_family", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 13L, "view_family", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 23L, "add_edit_scale_category", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 8L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 12L, "delete_external_scale", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 10L, "view_external_scale", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 134L, "associate_category_image", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, "Assign Image", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 9L, "delete_category", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 8L, "add_edit_category", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 7L, "view_category", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 6L, "delete_discount", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 5L, "add_edit_discount", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 4L, "view_discount", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 3L, "delete_tax", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 2L, "add_edit_tax", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 11L, "add_edit_external_scale", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 58L, "view_ad_batch", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 20L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 24L, "delete_scale_category", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 8L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 25L, "view_scale_label_type", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 56L, "add_edit_price_batch", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 19L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 55L, "view_price_batch", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 19L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 54L, "delete_role", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 18L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 53L, "add_edit_role", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 18L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 52L, "view_role", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 18L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 51L, "delete_user", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 17L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 50L, "add_edit_user", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 17L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 49L, "view_user", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 17L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 42L, "delete_vendor_contact", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 14L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 41L, "add_edit_vendor_contact", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 14L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 136L, "associate_scale_category_image", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 8L, "Assign Image", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 40L, "view_vendor_contact", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 14L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 38L, "add_edit_vendor", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 13L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 37L, "view_vendor", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 13L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 36L, "delete_tender_type", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 35L, "add_edit_tender_type", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 34L, "view_tender_type", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 30L, "delete_store", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 10L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 29L, "add_edit_store", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 10L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 28L, "view_store", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 10L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 27L, "delete_scale_label_type", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 26L, "add_edit_scale_label_type", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, "Add/Edit", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 39L, "delete_vendor", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 13L, "Delete", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" },
                    { 1L, "view_tax", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, "View", true, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6" }
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Address1", "Address2", "CityId", "CityName", "CountryId", "CountryName", "CreatedAt", "Email", "FirstName", "LastName", "Phone", "ProvinceId", "ProvinceName", "State", "StoreCredit", "TaxExcept", "UpdatedAt", "UserCreatorId", "Zip" },
                values: new object[,]
                {
                    { 1L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName0", " CustomerLastName0", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null },
                    { 2L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName1", " CustomerLastName1", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null },
                    { 3L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName2", " CustomerLastName2", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null },
                    { 4L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName3", " CustomerLastName3", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null },
                    { 5L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName4", " CustomerLastName4", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null },
                    { 6L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName5", " CustomerLastName5", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null },
                    { 7L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName6", " CustomerLastName6", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null },
                    { 8L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName7", " CustomerLastName7", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null },
                    { 9L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName8", " CustomerLastName8", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null },
                    { 10L, null, null, 1L, null, 0L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "name@company.com", "CustomerFirstName9", " CustomerLastName9", "1234567890", 0L, null, true, 0m, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a54f8cb3-1d5f-424c-aaa2-11e39c14f7f6", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batch_StoreId",
                table: "Batch",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_BOUser_BOProfileId",
                table: "BOUser",
                column: "BOProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_BOUser_POSProfileId",
                table: "BOUser",
                column: "POSProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_BOUser_RoleId",
                table: "BOUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_BOUser_UserId",
                table: "BOUser",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryId",
                table: "Category",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_DefaulShelfTagId",
                table: "Category",
                column: "DefaulShelfTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_DepartmentId",
                table: "Category",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFee_FeesId",
                table: "CategoryFee",
                column: "FeesId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTax_TaxsId",
                table: "CategoryTax",
                column: "TaxsId");

            migrationBuilder.CreateIndex(
                name: "IX_City_Name",
                table: "City",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_City_ProvinceId",
                table: "City",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientApplication_Name",
                table: "ClientApplication",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_Name",
                table: "Country",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CityId",
                table: "Customer",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_DepartmentId",
                table: "Department",
                column: "DepartmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Department_Name",
                table: "Department",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Device_GuidId",
                table: "Device",
                column: "GuidId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Device_StoreId",
                table: "Device",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_CategoryId",
                table: "Discount",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_DepartmentId",
                table: "Discount",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_FamilyId",
                table: "Discount",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_Name",
                table: "Discount",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProduct_ProductsId",
                table: "DiscountProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CityId",
                table: "Employee",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStore_StoresId",
                table: "EmployeeStore",
                column: "StoresId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalScale_ScaleBrandId",
                table: "ExternalScale",
                column: "ScaleBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalScale_StoreId",
                table: "ExternalScale",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyFee_FeesId",
                table: "FamilyFee",
                column: "FeesId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMixAndMatch_MixAndMatchsId",
                table: "FamilyMixAndMatch",
                column: "MixAndMatchsId");

            migrationBuilder.CreateIndex(
                name: "IX_Fee_Name",
                table: "Fee",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeeProduct_ProductsId",
                table: "FeeProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionGroup_ClientApplicationId",
                table: "FunctionGroup",
                column: "ClientApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionGroup_Name",
                table: "FunctionGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FunctionGroup_ProfilesId",
                table: "FunctionGroup",
                column: "ProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_KitProductProduct_KitProductId",
                table: "KitProductProduct",
                column: "KitProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MixAndMatch_Name",
                table: "MixAndMatch",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MixAndMatch_ProductBuyId",
                table: "MixAndMatch",
                column: "ProductBuyId");

            migrationBuilder.CreateIndex(
                name: "IX_MixAndMatchProduct_ProductsId",
                table: "MixAndMatchProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_FunctionGroupId",
                table: "Permission",
                column: "FunctionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionProfiles_ProfilesId",
                table: "PermissionProfiles",
                column: "ProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceBatchDetail_CategoryId",
                table: "PriceBatchDetail",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceBatchDetail_FamilyId",
                table: "PriceBatchDetail",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceBatchDetail_HeaderId",
                table: "PriceBatchDetail",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceBatchDetail_ProductId",
                table: "PriceBatchDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_DefaulShelfTagId",
                table: "Product",
                column: "DefaulShelfTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_DepartmentId",
                table: "Product",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_FamilyId",
                table: "Product",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Name",
                table: "Product",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_UPC",
                table: "Product",
                column: "UPC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ApplicationId",
                table: "Profiles",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Name",
                table: "Profiles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Province_CountryId",
                table: "Province",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Province_Name",
                table: "Province",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Region_Name",
                table: "Region",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_GuidId",
                table: "Report",
                column: "GuidId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_Name",
                table: "Report",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_RegionId",
                table: "Role",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleBrand_Name",
                table: "ScaleBrand",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScaleCategory_CategoryId",
                table: "ScaleCategory",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScaleCategory_DepartmentId",
                table: "ScaleCategory",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleCategory_Name",
                table: "ScaleCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScaleHomeFav_DepartmentId",
                table: "ScaleHomeFav",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleHomeFav_StoreId_DepartmentId",
                table: "ScaleHomeFav",
                columns: new[] { "StoreId", "DepartmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScaleHomeFavScaleProduct_ScaleProductsId",
                table: "ScaleHomeFavScaleProduct",
                column: "ScaleProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleLabelDefinition_ScaleBrandId",
                table: "ScaleLabelDefinition",
                column: "ScaleBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleLabelDefinition_ScaleLabelType1Id",
                table: "ScaleLabelDefinition",
                column: "ScaleLabelType1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleLabelDefinition_ScaleLabelType2Id",
                table: "ScaleLabelDefinition",
                column: "ScaleLabelType2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleLabelDefinition_ScaleProductId",
                table: "ScaleLabelDefinition",
                column: "ScaleProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleLabelType_Name",
                table: "ScaleLabelType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScaleProduct_PLUNumber",
                table: "ScaleProduct",
                column: "PLUNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScaleProduct_ScaleCategoryId",
                table: "ScaleProduct",
                column: "ScaleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_GuidId",
                table: "Store",
                column: "GuidId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_Name",
                table: "Store",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_RegionId",
                table: "Store",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_RoleId",
                table: "Store",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProduct_ProductId_StoreId",
                table: "StoreProduct",
                columns: new[] { "ProductId", "StoreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreProduct_StoreId",
                table: "StoreProduct",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductTax_TaxsId",
                table: "StoreProductTax",
                column: "TaxsId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTax_TaxsId",
                table: "StoreTax",
                column: "TaxsId");

            migrationBuilder.CreateIndex(
                name: "IX_Synchro_StoreId",
                table: "Synchro",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_SynchroDetail_SynchroId",
                table: "SynchroDetail",
                column: "SynchroId");

            migrationBuilder.CreateIndex(
                name: "IX_Tax_Name",
                table: "Tax",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenderType_Name",
                table: "TenderType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_GuidId",
                table: "Ticket",
                column: "GuidId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorContact_VendorId",
                table: "VendorContact",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorProduct_ProductId",
                table: "VendorProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorProduct_VendorId",
                table: "VendorProduct",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOUser");

            migrationBuilder.DropTable(
                name: "CategoryFee");

            migrationBuilder.DropTable(
                name: "CategoryTax");

            migrationBuilder.DropTable(
                name: "CSVMapping");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "DiscountProduct");

            migrationBuilder.DropTable(
                name: "EmployeeStore");

            migrationBuilder.DropTable(
                name: "ExternalScale");

            migrationBuilder.DropTable(
                name: "FamilyFee");

            migrationBuilder.DropTable(
                name: "FamilyMixAndMatch");

            migrationBuilder.DropTable(
                name: "FeeProduct");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "KitProductProduct");

            migrationBuilder.DropTable(
                name: "MixAndMatchProduct");

            migrationBuilder.DropTable(
                name: "PermissionProfiles");

            migrationBuilder.DropTable(
                name: "PriceBatchDetail");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "ScaleHomeFavScaleProduct");

            migrationBuilder.DropTable(
                name: "ScaleLabelDefinition");

            migrationBuilder.DropTable(
                name: "StoreProductTax");

            migrationBuilder.DropTable(
                name: "StoreTax");

            migrationBuilder.DropTable(
                name: "SynchroDetail");

            migrationBuilder.DropTable(
                name: "TenderType");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "VendorContact");

            migrationBuilder.DropTable(
                name: "VendorProduct");

            migrationBuilder.DropTable(
                name: "WProduct");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Fee");

            migrationBuilder.DropTable(
                name: "KitProduct");

            migrationBuilder.DropTable(
                name: "MixAndMatch");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Batch");

            migrationBuilder.DropTable(
                name: "ScaleHomeFav");

            migrationBuilder.DropTable(
                name: "ScaleBrand");

            migrationBuilder.DropTable(
                name: "ScaleProduct");

            migrationBuilder.DropTable(
                name: "StoreProduct");

            migrationBuilder.DropTable(
                name: "Tax");

            migrationBuilder.DropTable(
                name: "Synchro");

            migrationBuilder.DropTable(
                name: "Vendor");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "FunctionGroup");

            migrationBuilder.DropTable(
                name: "ScaleCategory");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Family");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "ClientApplication");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "ScaleLabelType");

            migrationBuilder.DropTable(
                name: "Region");
        }
    }
}
