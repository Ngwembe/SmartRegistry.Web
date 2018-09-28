using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartRegistry.Web.Data.Migrations
{
    public partial class AddedAnnouncementTypeOwneridOnAnnouncement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Announcements",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "AnnouncementTypeOwnerId",
                table: "Announcements",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnouncementTypeOwnerId",
                table: "Announcements");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Announcements",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
