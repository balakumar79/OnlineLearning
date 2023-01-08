using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class Modified_Subject_SubTopics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSubTopic_SubjectTopics_SubjectTopicId",
                table: "SubjectSubTopic");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTopics_TestSubjects_TestSubjectId",
                table: "SubjectTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectSubTopic",
                table: "SubjectSubTopic");

            migrationBuilder.RenameTable(
                name: "SubjectSubTopic",
                newName: "SubjectSubTopics");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectSubTopic_SubjectTopicId",
                table: "SubjectSubTopics",
                newName: "IX_SubjectSubTopics_SubjectTopicId");

            migrationBuilder.AlterColumn<int>(
                name: "TestSubjectId",
                table: "SubjectTopics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectTopicId",
                table: "SubjectSubTopics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectSubTopics",
                table: "SubjectSubTopics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSubTopics_SubjectTopics_SubjectTopicId",
                table: "SubjectSubTopics",
                column: "SubjectTopicId",
                principalTable: "SubjectTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTopics_TestSubjects_TestSubjectId",
                table: "SubjectTopics",
                column: "TestSubjectId",
                principalTable: "TestSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSubTopics_SubjectTopics_SubjectTopicId",
                table: "SubjectSubTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTopics_TestSubjects_TestSubjectId",
                table: "SubjectTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectSubTopics",
                table: "SubjectSubTopics");

            migrationBuilder.RenameTable(
                name: "SubjectSubTopics",
                newName: "SubjectSubTopic");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectSubTopics_SubjectTopicId",
                table: "SubjectSubTopic",
                newName: "IX_SubjectSubTopic_SubjectTopicId");

            migrationBuilder.AlterColumn<int>(
                name: "TestSubjectId",
                table: "SubjectTopics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectTopicId",
                table: "SubjectSubTopic",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectSubTopic",
                table: "SubjectSubTopic",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSubTopic_SubjectTopics_SubjectTopicId",
                table: "SubjectSubTopic",
                column: "SubjectTopicId",
                principalTable: "SubjectTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTopics_TestSubjects_TestSubjectId",
                table: "SubjectTopics",
                column: "TestSubjectId",
                principalTable: "TestSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
