using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaparaApp.Project.API.Migrations
{
    /// <inheritdoc />
    public partial class added_discount_history_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userDiscountStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDiscountActive = table.Column<bool>(type: "bit", nullable: false),
                    DiscountStartYear = table.Column<int>(type: "int", nullable: false),
                    DiscountEndYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDiscountStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTimelyPaymentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimelyPaidWater = table.Column<int>(type: "int", nullable: false),
                    TimelyPaidElectricity = table.Column<int>(type: "int", nullable: false),
                    TimelyPaidGas = table.Column<int>(type: "int", nullable: false),
                    TimelyPaidRent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTimelyPaymentDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userDiscountStatuses");

            migrationBuilder.DropTable(
                name: "UserTimelyPaymentDetails");
        }
    }
}
