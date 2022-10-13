using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class Added_Comperhension_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionQusID",
                table: "Comprehensions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestSectionId",
                table: "Comprehensions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comprehensions_QuestionQusID",
                table: "Comprehensions",
                column: "QuestionQusID");

            migrationBuilder.CreateIndex(
                name: "IX_Comprehensions_TestId",
                table: "Comprehensions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprehensions_TestSectionId",
                table: "Comprehensions",
                column: "TestSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comprehensions_Questions_QuestionQusID",
                table: "Comprehensions",
                column: "QuestionQusID",
                principalTable: "Questions",
                principalColumn: "QusID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comprehensions_Tests_TestId",
                table: "Comprehensions",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comprehensions_TestSections_TestSectionId",
                table: "Comprehensions",
                column: "TestSectionId",
                principalTable: "TestSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comprehensions_Questions_QuestionQusID",
                table: "Comprehensions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comprehensions_Tests_TestId",
                table: "Comprehensions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comprehensions_TestSections_TestSectionId",
                table: "Comprehensions");

            migrationBuilder.DropIndex(
                name: "IX_Comprehensions_QuestionQusID",
                table: "Comprehensions");

            migrationBuilder.DropIndex(
                name: "IX_Comprehensions_TestId",
                table: "Comprehensions");

            migrationBuilder.DropIndex(
                name: "IX_Comprehensions_TestSectionId",
                table: "Comprehensions");

            migrationBuilder.DropColumn(
                name: "QuestionQusID",
                table: "Comprehensions");

            migrationBuilder.DropColumn(
                name: "TestSectionId",
                table: "Comprehensions");
        }
    }
}
