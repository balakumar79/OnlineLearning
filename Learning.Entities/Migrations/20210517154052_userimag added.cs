using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class userimagadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserProfileImage",
                schema: "Identity",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTopics",
                schema: "Identity",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Topics",
                schema: "Identity",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserProfileImage",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SubTopics",
                schema: "Identity",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Topics",
                schema: "Identity",
                table: "Tests");
        }
    }
}
