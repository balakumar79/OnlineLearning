using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class addedFK_Question_Topicid_Subtopicid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Questions_SubTopicId",
                table: "Questions",
                column: "SubTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TopicId",
                table: "Questions",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_SubjectSubTopics_SubTopicId",
                table: "Questions",
                column: "SubTopicId",
                principalTable: "SubjectSubTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_SubjectTopics_TopicId",
                table: "Questions",
                column: "TopicId",
                principalTable: "SubjectTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_SubjectSubTopics_SubTopicId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_SubjectTopics_TopicId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SubTopicId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TopicId",
                table: "Questions");
        }
    }
}
