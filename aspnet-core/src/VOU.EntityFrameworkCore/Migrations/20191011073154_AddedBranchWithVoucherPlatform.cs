using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VOU.Migrations
{
    public partial class AddedBranchWithVoucherPlatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchWithVoucherPlatform",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    VoucherPlatformId = table.Column<int>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    ArchivedTime = table.Column<DateTime>(nullable: true),
                    ArchivedByUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchWithVoucherPlatform", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchWithVoucherPlatform_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchWithVoucherPlatform_AbpTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "AbpTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchWithVoucherPlatform_VoucherPlatforms_VoucherPlatformId",
                        column: x => x.VoucherPlatformId,
                        principalTable: "VoucherPlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchWithVoucherPlatform_LocationId",
                table: "BranchWithVoucherPlatform",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchWithVoucherPlatform_TenantId",
                table: "BranchWithVoucherPlatform",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchWithVoucherPlatform_VoucherPlatformId",
                table: "BranchWithVoucherPlatform",
                column: "VoucherPlatformId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchWithVoucherPlatform");
        }
    }
}
