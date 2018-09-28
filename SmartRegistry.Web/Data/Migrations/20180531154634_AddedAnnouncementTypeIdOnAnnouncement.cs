using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartRegistry.Web.Data.Migrations
{
    public partial class AddedAnnouncementTypeIdOnAnnouncement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnouncementTypeId",
                table: "Announcements",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnouncementTypeId",
                table: "Announcements");
        }
    }
}
