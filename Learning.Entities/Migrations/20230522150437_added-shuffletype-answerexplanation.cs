using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class addedshuffletypeanswerexplanation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShuffleTypeId",
                //schema: "bala",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AnswerExplantion",
                //schema: "bala",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShuffleTypeId",
                schema: "bala",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "AnswerExplantion",
                schema: "bala",
                table: "Questions");
        }
    }
}
