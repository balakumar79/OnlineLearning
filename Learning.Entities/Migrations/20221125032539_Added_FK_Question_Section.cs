using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class Added_FK_Question_Section : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Topics",
                table: "Questions",
                newName: "TopicId");

            migrationBuilder.RenameColumn(
                name: "SubTopics",
                table: "Questions",
                newName: "SubTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SectionId",
                table: "Questions",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TestSections_SectionId",
                table: "Questions",
                column: "SectionId",
                principalTable: "TestSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TestSections_SectionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SectionId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "Questions",
                newName: "Topics");

            migrationBuilder.RenameColumn(
                name: "SubTopicId",
                table: "Questions",
                newName: "SubTopics");
        }
    }
}
