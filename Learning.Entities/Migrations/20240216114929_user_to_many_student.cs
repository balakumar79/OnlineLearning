using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class user_to_many_student : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Student_UserID",
                schema: "bala",
                table: "Student");

            migrationBuilder.CreateIndex(
                name: "IX_Student_UserID",
                schema: "bala",
                table: "Student",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Student_UserID",
                schema: "bala",
                table: "Student");

            migrationBuilder.CreateIndex(
                name: "IX_Student_UserID",
                schema: "bala",
                table: "Student",
                column: "UserID",
                unique: true);
        }
    }
}
