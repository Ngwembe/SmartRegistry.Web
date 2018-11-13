using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartRegistry.Web.Migrations
{
    public partial class AddedDescriptionToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Subject",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Faculty",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Department",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Course",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Faculty");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Course");
        }
    }
}
