using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class renamed_columns_Question : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Questions",
                newName: "TestStatusId");

            migrationBuilder.RenameColumn(
                name: "QusType",
                table: "Questions",
                newName: "QuestionTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "Comprehensions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestStatusId",
                table: "Questions",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "QuestionTypeId",
                table: "Questions",
                newName: "QusType");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "Comprehensions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
