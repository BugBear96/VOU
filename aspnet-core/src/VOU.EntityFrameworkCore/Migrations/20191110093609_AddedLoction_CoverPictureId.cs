using Microsoft.EntityFrameworkCore.Migrations;

namespace VOU.Migrations
{
    public partial class AddedLoction_CoverPictureId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CoverPictureId",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPictureId",
                table: "Location");
        }
    }
}
