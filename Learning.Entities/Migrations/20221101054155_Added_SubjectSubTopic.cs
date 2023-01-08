using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class Added_SubjectSubTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubjectSubTopic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    SubTopic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectTopicId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectSubTopic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectSubTopic_SubjectTopics_SubjectTopicId",
                        column: x => x.SubjectTopicId,
                        principalTable: "SubjectTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSubTopic_SubjectTopicId",
                table: "SubjectSubTopic",
                column: "SubjectTopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectSubTopic");
        }
    }
}
