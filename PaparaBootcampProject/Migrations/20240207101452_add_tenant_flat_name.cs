using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaparaApp.Project.API.Migrations
{
    /// <inheritdoc />
    public partial class add_tenant_flat_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tenantFlats_AspNetUsers_TenantId",
                table: "tenantFlats");

            migrationBuilder.DropForeignKey(
                name: "FK_tenantFlats_Flats_FlatId",
                table: "tenantFlats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tenantFlats",
                table: "tenantFlats");

            migrationBuilder.RenameTable(
                name: "tenantFlats",
                newName: "TenantFlats");

            migrationBuilder.RenameIndex(
                name: "IX_tenantFlats_TenantId",
                table: "TenantFlats",
                newName: "IX_TenantFlats_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_tenantFlats_FlatId",
                table: "TenantFlats",
                newName: "IX_TenantFlats_FlatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantFlats",
                table: "TenantFlats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantFlats_AspNetUsers_TenantId",
                table: "TenantFlats",
                column: "TenantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantFlats_Flats_FlatId",
                table: "TenantFlats",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantFlats_AspNetUsers_TenantId",
                table: "TenantFlats");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantFlats_Flats_FlatId",
                table: "TenantFlats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantFlats",
                table: "TenantFlats");

            migrationBuilder.RenameTable(
                name: "TenantFlats",
                newName: "tenantFlats");

            migrationBuilder.RenameIndex(
                name: "IX_TenantFlats_TenantId",
                table: "tenantFlats",
                newName: "IX_tenantFlats_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantFlats_FlatId",
                table: "tenantFlats",
                newName: "IX_tenantFlats_FlatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tenantFlats",
                table: "tenantFlats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tenantFlats_AspNetUsers_TenantId",
                table: "tenantFlats",
                column: "TenantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tenantFlats_Flats_FlatId",
                table: "tenantFlats",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
