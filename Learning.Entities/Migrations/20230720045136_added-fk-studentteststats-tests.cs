using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class addedfkstudentteststatstests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentTestStats_Testid",
                schema: "bala",
                table: "StudentTestStats");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestStats_Testid",
                schema: "bala",
                table: "StudentTestStats",
                column: "Testid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentTestStats_Testid",
                schema: "bala",
                table: "StudentTestStats");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestStats_Testid",
                schema: "bala",
                table: "StudentTestStats",
                column: "Testid");
        }
    }
}
