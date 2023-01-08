using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class added_subjectLanguageVariant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestSubjects_Languages_LanguageId",
                table: "TestSubjects");

            migrationBuilder.DropIndex(
                name: "IX_TestSubjects_LanguageId",
                table: "TestSubjects");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "TestSubjects");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "TestSubjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Languages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Languages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "SubjectLanguageVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectLanguageVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectLanguageVariants_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectLanguageVariants_TestSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "TestSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_LanguageId",
                table: "Tests",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLanguageVariants_LanguageId",
                table: "SubjectLanguageVariants",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLanguageVariants_SubjectId",
                table: "SubjectLanguageVariants",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Languages_LanguageId",
                table: "Tests",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Languages_LanguageId",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "SubjectLanguageVariants");

            migrationBuilder.DropIndex(
                name: "IX_Tests_LanguageId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "TestSubjects");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Languages");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "TestSubjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TestSubjects_LanguageId",
                table: "TestSubjects",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSubjects_Languages_LanguageId",
                table: "TestSubjects",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
