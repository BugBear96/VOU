using Microsoft.EntityFrameworkCore.Migrations;

namespace VOU.Migrations
{
    public partial class AddedTenant_Categry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "TenantSubCategory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "AbpTenants",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubCategory_TenantId",
                table: "TenantSubCategory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_CategoryId",
                table: "AbpTenants",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpTenants_TenantCategory_CategoryId",
                table: "AbpTenants",
                column: "CategoryId",
                principalTable: "TenantCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSubCategory_AbpTenants_TenantId",
                table: "TenantSubCategory",
                column: "TenantId",
                principalTable: "AbpTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpTenants_TenantCategory_CategoryId",
                table: "AbpTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantSubCategory_AbpTenants_TenantId",
                table: "TenantSubCategory");

            migrationBuilder.DropIndex(
                name: "IX_TenantSubCategory_TenantId",
                table: "TenantSubCategory");

            migrationBuilder.DropIndex(
                name: "IX_AbpTenants_CategoryId",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "TenantSubCategory");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AbpTenants");
        }
    }
}
