using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class addsudenstats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentTestStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Testid = table.Column<int>(type: "int", nullable: false),
                    MaximumMarkScored = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumMarkScored = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageMarkScored = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfAttempts = table.Column<int>(type: "int", nullable: false),
                    TotalRegistration = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTestStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentTestStats_Tests_Testid",
                        column: x => x.Testid,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestStats_Testid",
                table: "StudentTestStats",
                column: "Testid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTestStats");
        }
    }
}
