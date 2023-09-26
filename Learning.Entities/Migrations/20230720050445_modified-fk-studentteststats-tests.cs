using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class modifiedfkstudentteststatstests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTestStats_Tests_Testid",
                schema: "bala",
                table: "StudentTestStats");

            migrationBuilder.RenameColumn(
                name: "Testid",
                schema: "bala",
                table: "StudentTestStats",
                newName: "TestId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentTestStats_Testid",
                schema: "bala",
                table: "StudentTestStats",
                newName: "IX_StudentTestStats_TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTestStats_Tests_TestId",
                schema: "bala",
                table: "StudentTestStats",
                column: "TestId",
                principalSchema: "bala",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTestStats_Tests_TestId",
                schema: "bala",
                table: "StudentTestStats");

            migrationBuilder.RenameColumn(
                name: "TestId",
                schema: "bala",
                table: "StudentTestStats",
                newName: "Testid");

            migrationBuilder.RenameIndex(
                name: "IX_StudentTestStats_TestId",
                schema: "bala",
                table: "StudentTestStats",
                newName: "IX_StudentTestStats_Testid");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTestStats_Tests_Testid",
                schema: "bala",
                table: "StudentTestStats",
                column: "Testid",
                principalSchema: "bala",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
