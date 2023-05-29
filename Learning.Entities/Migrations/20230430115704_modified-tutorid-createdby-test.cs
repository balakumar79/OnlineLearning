using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class modifiedtutoridcreatedbytest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Tutors_TutorId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TutorId",
                table: "Tests");

            migrationBuilder.RenameColumn(
                name: "TutorId",
                table: "Tests",
                newName: "CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Tests",
                newName: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TutorId",
                table: "Tests",
                column: "TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Tutors_TutorId",
                table: "Tests",
                column: "TutorId",
                principalTable: "Tutors",
                principalColumn: "TutorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
