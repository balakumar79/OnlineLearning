using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class addfkteacheruser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_User_Id1",
                schema: "bala",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_Id1",
                schema: "bala",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Id1",
                schema: "bala",
                table: "Teachers");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                schema: "bala",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_User_UserId",
                schema: "bala",
                table: "Teachers",
                column: "UserId",
                principalSchema: "bala",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_User_UserId",
                schema: "bala",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_UserId",
                schema: "bala",
                table: "Teachers");

            migrationBuilder.AddColumn<int>(
                name: "Id1",
                schema: "bala",
                table: "Teachers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Id1",
                schema: "bala",
                table: "Teachers",
                column: "Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_User_Id1",
                schema: "bala",
                table: "Teachers",
                column: "Id1",
                principalSchema: "bala",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
