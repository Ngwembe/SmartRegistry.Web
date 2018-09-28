using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartRegistry.Web.Data.Migrations
{
    public partial class RemovedAnnouncementTypeEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnouncementType",
                table: "Announcements");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Announcements",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Announcements",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "AnnouncementType",
                table: "Announcements",
                nullable: false,
                defaultValue: 0);
        }
    }
}
