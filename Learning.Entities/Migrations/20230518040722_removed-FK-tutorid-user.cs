using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class removedFKtutoriduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Tutors_TutorId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_TutorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TutorId",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TutorId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_TutorId",
                table: "User",
                column: "TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Tutors_TutorId",
                table: "User",
                column: "TutorId",
                principalTable: "Tutors",
                principalColumn: "TutorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
