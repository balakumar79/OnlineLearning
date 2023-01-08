using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class added_language_TestSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "TestSubjects",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.CreateIndex(
                name: "IX_TestSubjects_LanguageId",
                table: "TestSubjects",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSubjects_Languages_LanguageId",
                table: "TestSubjects",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestSubjects_Languages_LanguageId",
                table: "TestSubjects");

            migrationBuilder.DropIndex(
                name: "IX_TestSubjects_LanguageId",
                table: "TestSubjects");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "TestSubjects");
        }
    }
}
