using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class Added_FK_Questions_Test_Section_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeID",
                table: "Tests");

            migrationBuilder.RenameColumn(
                name: "SubjectID",
                table: "Tests",
                newName: "TestSubjectId");

            migrationBuilder.RenameColumn(
                name: "StatusID",
                table: "Tests",
                newName: "TestStatusId");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "Tests",
                newName: "GradeLevelsId");

            migrationBuilder.AlterColumn<int>(
                name: "TutorId",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Topics",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubTopics",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestStatusId",
                table: "Tests",
                column: "TestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestSubjectId",
                table: "Tests",
                column: "TestSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TutorId",
                table: "Tests",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestStatusId",
                table: "Questions",
                column: "TestStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId",
                principalTable: "QuestionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TestStatuses_TestStatusId",
                table: "Questions",
                column: "TestStatusId",
                principalTable: "TestStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestStatuses_TestStatusId",
                table: "Tests",
                column: "TestStatusId",
                principalTable: "TestStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestSubjects_TestSubjectId",
                table: "Tests",
                column: "TestSubjectId",
                principalTable: "TestSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Tutors_TutorId",
                table: "Tests",
                column: "TutorId",
                principalTable: "Tutors",
                principalColumn: "TutorId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TestStatuses_TestStatusId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestStatuses_TestStatusId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestSubjects_TestSubjectId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Tutors_TutorId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TestStatusId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TestSubjectId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TutorId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TestStatusId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "TestSubjectId",
                table: "Tests",
                newName: "SubjectID");

            migrationBuilder.RenameColumn(
                name: "TestStatusId",
                table: "Tests",
                newName: "StatusID");

            migrationBuilder.RenameColumn(
                name: "GradeLevelsId",
                table: "Tests",
                newName: "Language");

            migrationBuilder.AlterColumn<string>(
                name: "TutorId",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GradeID",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Topics",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SubTopics",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
