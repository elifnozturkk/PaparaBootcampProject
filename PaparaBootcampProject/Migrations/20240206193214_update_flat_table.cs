using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaparaApp.Project.API.Migrations
{
    /// <inheritdoc />
    public partial class update_flat_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flats_AspNetUsers_TenantId",
                table: "Flats");

            migrationBuilder.DropIndex(
                name: "IX_Flats_TenantId",
                table: "Flats");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Flats",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_TenantId",
                table: "Flats",
                column: "TenantId",
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_AspNetUsers_TenantId",
                table: "Flats",
                column: "TenantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flats_AspNetUsers_TenantId",
                table: "Flats");

            migrationBuilder.DropIndex(
                name: "IX_Flats_TenantId",
                table: "Flats");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Flats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flats_TenantId",
                table: "Flats",
                column: "TenantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_AspNetUsers_TenantId",
                table: "Flats",
                column: "TenantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
