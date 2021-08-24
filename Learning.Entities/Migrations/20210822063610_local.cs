using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class local : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HearAbout",
                table: "Tutors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentTestHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentTestId = table.Column<int>(type: "int", nullable: false),
                    AttemptedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeTaken = table.Column<TimeSpan>(type: "time", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    Marks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTestHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    Assigner = table.Column<int>(type: "int", nullable: false),
                    AssignedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTestHistories");

            migrationBuilder.DropTable(
                name: "StudentTests");

            migrationBuilder.DropColumn(
                name: "District",
                table: "User");

            migrationBuilder.DropColumn(
                name: "HearAbout",
                table: "Tutors");
        }
    }
}
