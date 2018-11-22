using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartRegistry.Web.Migrations
{
    public partial class AddedColorThemeOnSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorTheme",
                table: "Schedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorTheme",
                table: "Schedule");
        }
    }
}
