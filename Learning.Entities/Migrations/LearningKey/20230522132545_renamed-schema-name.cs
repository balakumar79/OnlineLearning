using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations.LearningKey
{
    public partial class renamedschemaname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DME");

            migrationBuilder.RenameTable(
                name: "DataProtectionKeys",
                newName: "DataProtectionKeys",
                newSchema: "DME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "DataProtectionKeys",
                schema: "DME",
                newName: "DataProtectionKeys");
        }
    }
}
