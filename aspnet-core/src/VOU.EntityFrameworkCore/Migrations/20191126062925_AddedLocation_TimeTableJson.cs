using Microsoft.EntityFrameworkCore.Migrations;

namespace VOU.Migrations
{
    public partial class AddedLocation_TimeTableJson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeTableJson",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeTableJson",
                table: "Location");
        }
    }
}
