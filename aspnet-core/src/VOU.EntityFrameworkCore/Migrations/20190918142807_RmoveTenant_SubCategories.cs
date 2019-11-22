using Microsoft.EntityFrameworkCore.Migrations;

namespace VOU.Migrations
{
    public partial class RmoveTenant_SubCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantSubCategory_AbpTenants_TenantId",
                table: "TenantSubCategory");

            migrationBuilder.DropIndex(
                name: "IX_TenantSubCategory_TenantId",
                table: "TenantSubCategory");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "TenantSubCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "TenantSubCategory",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubCategory_TenantId",
                table: "TenantSubCategory",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSubCategory_AbpTenants_TenantId",
                table: "TenantSubCategory",
                column: "TenantId",
                principalTable: "AbpTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
