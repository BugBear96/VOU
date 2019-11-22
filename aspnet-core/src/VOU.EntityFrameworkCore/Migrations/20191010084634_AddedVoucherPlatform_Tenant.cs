using Microsoft.EntityFrameworkCore.Migrations;

namespace VOU.Migrations
{
    public partial class AddedVoucherPlatform_Tenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VoucherPlatforms_TenantId",
                table: "VoucherPlatforms",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherPlatforms_AbpTenants_TenantId",
                table: "VoucherPlatforms",
                column: "TenantId",
                principalTable: "AbpTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherPlatforms_AbpTenants_TenantId",
                table: "VoucherPlatforms");

            migrationBuilder.DropIndex(
                name: "IX_VoucherPlatforms_TenantId",
                table: "VoucherPlatforms");
        }
    }
}
