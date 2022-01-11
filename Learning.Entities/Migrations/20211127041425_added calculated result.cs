using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class addedcalculatedresult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Marks",
                table: "StudentTestHistories",
                newName: "TotalMarks");

            migrationBuilder.AlterColumn<string>(
                name: "CorrectAnswers",
                table: "StudentTestHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "StudentTestHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "StudentTestHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentTestId",
                table: "StudentAnswerLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CalculatedResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    StudentTestId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CalculatedResults = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedResults_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalculatedResults_StudentTests_StudentTestId",
                        column: x => x.StudentTestId,
                        principalTable: "StudentTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswerLogs_StudentTestId",
                table: "StudentAnswerLogs",
                column: "StudentTestId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedResults_StudentId",
                table: "CalculatedResults",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedResults_StudentTestId",
                table: "CalculatedResults",
                column: "StudentTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswerLogs_StudentTests_StudentTestId",
                table: "StudentAnswerLogs",
                column: "StudentTestId",
                principalTable: "StudentTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswerLogs_StudentTests_StudentTestId",
                table: "StudentAnswerLogs");

            migrationBuilder.DropTable(
                name: "CalculatedResults");

            migrationBuilder.DropIndex(
                name: "IX_StudentAnswerLogs_StudentTestId",
                table: "StudentAnswerLogs");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "StudentTestHistories");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentTestHistories");

            migrationBuilder.DropColumn(
                name: "StudentTestId",
                table: "StudentAnswerLogs");

            migrationBuilder.RenameColumn(
                name: "TotalMarks",
                table: "StudentTestHistories",
                newName: "Marks");

            migrationBuilder.AlterColumn<int>(
                name: "CorrectAnswers",
                table: "StudentTestHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
