using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class modified_topics_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectTestId",
                table: "SubjectTopics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectTestId",
                table: "SubjectTopics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
