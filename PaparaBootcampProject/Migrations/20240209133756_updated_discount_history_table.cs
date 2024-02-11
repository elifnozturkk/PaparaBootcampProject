using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaparaApp.Project.API.Migrations
{
    /// <inheritdoc />
    public partial class updated_discount_history_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_userDiscountStatuses",
                table: "userDiscountStatuses");

            migrationBuilder.RenameTable(
                name: "userDiscountStatuses",
                newName: "UserDiscountStatuses");

            migrationBuilder.RenameColumn(
                name: "TimelyPaidRent",
                table: "UserTimelyPaymentDetails",
                newName: "TimelyPaidDue");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDiscountStatuses",
                table: "UserDiscountStatuses",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDiscountStatuses",
                table: "UserDiscountStatuses");

            migrationBuilder.RenameTable(
                name: "UserDiscountStatuses",
                newName: "userDiscountStatuses");

            migrationBuilder.RenameColumn(
                name: "TimelyPaidDue",
                table: "UserTimelyPaymentDetails",
                newName: "TimelyPaidRent");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userDiscountStatuses",
                table: "userDiscountStatuses",
                column: "Id");
        }
    }
}
