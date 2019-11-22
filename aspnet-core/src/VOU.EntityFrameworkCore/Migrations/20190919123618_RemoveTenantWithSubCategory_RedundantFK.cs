using Microsoft.EntityFrameworkCore.Migrations;

namespace VOU.Migrations
{
    public partial class RemoveTenantWithSubCategory_RedundantFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantWithSubCategory_TenantSubCategory_SubCategoryId",
                table: "TenantWithSubCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantWithSubCategory_AbpTenants_TenantId",
                table: "TenantWithSubCategory");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "TenantWithSubCategory",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryId",
                table: "TenantWithSubCategory",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_TenantWithSubCategory_TenantSubCategory_SubCategoryId",
                table: "TenantWithSubCategory",
                column: "SubCategoryId",
                principalTable: "TenantSubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantWithSubCategory_AbpTenants_TenantId",
                table: "TenantWithSubCategory",
                column: "TenantId",
                principalTable: "AbpTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantWithSubCategory_TenantSubCategory_SubCategoryId",
                table: "TenantWithSubCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantWithSubCategory_AbpTenants_TenantId",
                table: "TenantWithSubCategory");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "TenantWithSubCategory",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryId",
                table: "TenantWithSubCategory",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantWithSubCategory_TenantSubCategory_SubCategoryId",
                table: "TenantWithSubCategory",
                column: "SubCategoryId",
                principalTable: "TenantSubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantWithSubCategory_AbpTenants_TenantId",
                table: "TenantWithSubCategory",
                column: "TenantId",
                principalTable: "AbpTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
