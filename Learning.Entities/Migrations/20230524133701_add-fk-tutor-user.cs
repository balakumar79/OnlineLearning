using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class addfktutoruser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tutors_UserId",
                schema: "bala",
                table: "Tutors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutors_User_UserId",
                schema: "bala",
                table: "Tutors",
                column: "UserId",
                principalSchema: "bala",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutors_User_UserId",
                schema: "bala",
                table: "Tutors");

            migrationBuilder.DropIndex(
                name: "IX_Tutors_UserId",
                schema: "bala",
                table: "Tutors");
        }
    }
}
