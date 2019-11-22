using Microsoft.EntityFrameworkCore.Migrations;

namespace VOU.Migrations
{
    public partial class AddedTenant_ProfilePictueId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProfilePictureId",
                table: "AbpTenants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "AbpTenants");
        }
    }
}
