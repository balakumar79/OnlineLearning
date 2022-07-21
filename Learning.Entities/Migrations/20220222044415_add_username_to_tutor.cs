using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class add_username_to_tutor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTopic",
                table: "TestSections");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "TestSections");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Tutors",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Tutors");

            migrationBuilder.AddColumn<string>(
                name: "SubTopic",
                table: "TestSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "TestSections",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
