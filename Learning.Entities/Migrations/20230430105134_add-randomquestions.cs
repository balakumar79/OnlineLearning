using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class addrandomquestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestType",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentTestId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RandomQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandomQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RandomQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QusID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RandomQuestions_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "StudentTestTest",
                columns: table => new
                {
                    StudentTestsId = table.Column<int>(type: "int", nullable: false),
                    TestsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTestTest", x => new { x.StudentTestsId, x.TestsId });
                    table.ForeignKey(
                        name: "FK_StudentTestTest_StudentTests_StudentTestsId",
                        column: x => x.StudentTestsId,
                        principalTable: "StudentTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StudentTestTest_Tests_TestsId",
                        column: x => x.TestsId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_StudentTestId",
                table: "Student",
                column: "StudentTestId");

            migrationBuilder.CreateIndex(
                name: "IX_RandomQuestions_QuestionId",
                table: "RandomQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RandomQuestions_TestId",
                table: "RandomQuestions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestTest_TestsId",
                table: "StudentTestTest",
                column: "TestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_StudentTests_StudentTestId",
                table: "Student",
                column: "StudentTestId",
                principalTable: "StudentTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_StudentTests_StudentTestId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "RandomQuestions");

            migrationBuilder.DropTable(
                name: "StudentTestTest");

            migrationBuilder.DropIndex(
                name: "IX_Student_StudentTestId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "TestType",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "StudentTestId",
                table: "Student");
        }
    }
}
