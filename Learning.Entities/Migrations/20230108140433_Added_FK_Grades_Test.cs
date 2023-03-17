using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class Added_FK_Grades_Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "StudentAccountRecoveryAnswers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_GradeLevelsId",
                table: "Tests",
                column: "GradeLevelsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_GradeLevels_GradeLevelsId",
                table: "Tests",
                column: "GradeLevelsId",
                principalTable: "GradeLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_GradeLevels_GradeLevelsId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_GradeLevelsId",
                table: "Tests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "StudentAccountRecoveryAnswers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
