using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class altered_test_and_question : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTopics",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Topics",
                table: "Tests");

            migrationBuilder.AddColumn<string>(
                name: "SubTopics",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Topics",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTopics",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Topics",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "SubTopics",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Topics",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
