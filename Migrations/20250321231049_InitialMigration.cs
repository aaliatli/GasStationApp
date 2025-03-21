using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GasStationApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GasType = table.Column<int>(type: "int", nullable: false),
                    PricePerLiter = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserNameLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StationCode = table.Column<int>(type: "int", nullable: false),
                    StationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "SaleRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GasType = table.Column<int>(type: "int", nullable: false),
                    SoldFuel = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                });

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GasType = table.Column<int>(type: "int", nullable: false),
                    Occupancy = table.Column<double>(type: "float", nullable: false),
                    TotalCapacity = table.Column<double>(type: "float", nullable: false),
                    AddedFuel = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Storages_Users_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                });

            migrationBuilder.InsertData(
                table: "FuelPrices",
                columns: new[] { "Id", "GasType", "PricePerLiter" },
                values: new object[,]
                {
                    { 1, 1, 20.5 },
                    { 2, 2, 18.300000000000001 },
                    { 3, 3, 22.100000000000001 },
                    { 4, 4, 21.0 },
                    { 5, 5, 19.5 }
                });

            migrationBuilder.InsertData(
                table: "Storages",
                columns: new[] { "Id", "AddedFuel", "GasType", "Occupancy", "TotalCapacity", "UserId", "UserModelId" },
                values: new object[,]
                {
                    { 1, 0.0, 1, 0.0, 10000.0, 0, null },
                    { 2, 0.0, 2, 0.0, 10000.0, 0, null },
                    { 3, 0.0, 3, 0.0, 10000.0, 0, null },
                    { 4, 0.0, 4, 0.0, 10000.0, 0, null },
                    { 5, 0.0, 5, 0.0, 10000.0, 0, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleRecords_UserId",
                table: "SaleRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_UserModelId",
                table: "Storages",
                column: "UserModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelPrices");

            migrationBuilder.DropTable(
                name: "SaleRecords");

            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
