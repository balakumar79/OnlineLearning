using Microsoft.EntityFrameworkCore.Migrations;

namespace Learning.Entities.Migrations
{
    public partial class Renameschemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "UserScreensAccess",
                schema: "bala",
                newName: "UserScreensAccess",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "bala",
                newName: "User",
                newSchema: "dbo");

            //migrationBuilder.RenameTable(
            //    name: "Tutors",
            //    schema: "bala",
            //    newName: "Tutors",
            //    newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TutorLanguageOfInstructions",
                schema: "bala",
                newName: "TutorLanguageOfInstructions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TutorGradesTakens",
                schema: "bala",
                newName: "TutorGradesTakens",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TutorEducations",
                schema: "bala",
                newName: "TutorEducations",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TrueOrFalses",
                schema: "bala",
                newName: "TrueOrFalses",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TestSubjects",
                schema: "bala",
                newName: "TestSubjects",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TestStatuses",
                schema: "bala",
                newName: "TestStatuses",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TestSections",
                schema: "bala",
                newName: "TestSections",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Tests",
                schema: "bala",
                newName: "Tests",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Teachers",
                schema: "bala",
                newName: "Teachers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SubjectTopics",
                schema: "bala",
                newName: "SubjectTopics",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SubjectSubTopics",
                schema: "bala",
                newName: "SubjectSubTopics",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SubjectLanguageVariants",
                schema: "bala",
                newName: "SubjectLanguageVariants",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StudentTestTest",
                schema: "bala",
                newName: "StudentTestTest",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StudentTestStats",
                schema: "bala",
                newName: "StudentTestStats",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StudentTests",
                schema: "bala",
                newName: "StudentTests",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StudentTestHistories",
                schema: "bala",
                newName: "StudentTestHistories",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StudentInvitations",
                schema: "bala",
                newName: "StudentInvitations",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StudentAnswerLogs",
                schema: "bala",
                newName: "StudentAnswerLogs",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StudentAccountRecoveryAnswers",
                schema: "bala",
                newName: "StudentAccountRecoveryAnswers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Student",
                schema: "bala",
                newName: "Student",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ScreenAccesses",
                schema: "bala",
                newName: "ScreenAccesses",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Role",
                schema: "bala",
                newName: "Role",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "RandomTests",
                schema: "bala",
                newName: "RandomTests",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "RandomQuestions",
                schema: "bala",
                newName: "RandomQuestions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "QuestionTypes",
                schema: "bala",
                newName: "QuestionTypes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Questions",
                schema: "bala",
                newName: "Questions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Options",
                schema: "bala",
                newName: "Options",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Notifications",
                schema: "bala",
                newName: "Notifications",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "MCQAnswers",
                schema: "bala",
                newName: "MCQAnswers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Mails",
                schema: "bala",
                newName: "Mails",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Loggers",
                schema: "bala",
                newName: "Loggers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "LanguageVariantQuestions",
                schema: "bala",
                newName: "LanguageVariantQuestions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Languages",
                schema: "bala",
                newName: "Languages",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GradeLevels",
                schema: "bala",
                newName: "GradeLevels",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Comprehensions",
                schema: "bala",
                newName: "Comprehensions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "CalculatedResults",
                schema: "bala",
                newName: "CalculatedResults",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "bala",
                newName: "AspNetUserTokens",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "bala",
                newName: "AspNetUserRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "bala",
                newName: "AspNetUserLogins",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "bala",
                newName: "AspNetUserClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "bala",
                newName: "AspNetRoleClaims",
                newSchema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bala");

            migrationBuilder.RenameTable(
                name: "UserScreensAccess",
                schema: "dbo",
                newName: "UserScreensAccess",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "dbo",
                newName: "User",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Tutors",
                schema: "dbo",
                newName: "Tutors",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "TutorLanguageOfInstructions",
                schema: "dbo",
                newName: "TutorLanguageOfInstructions",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "TutorGradesTakens",
                schema: "dbo",
                newName: "TutorGradesTakens",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "TutorEducations",
                schema: "dbo",
                newName: "TutorEducations",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "TrueOrFalses",
                schema: "dbo",
                newName: "TrueOrFalses",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "TestSubjects",
                schema: "dbo",
                newName: "TestSubjects",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "TestStatuses",
                schema: "dbo",
                newName: "TestStatuses",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "TestSections",
                schema: "dbo",
                newName: "TestSections",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Tests",
                schema: "dbo",
                newName: "Tests",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Teachers",
                schema: "dbo",
                newName: "Teachers",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "SubjectTopics",
                schema: "dbo",
                newName: "SubjectTopics",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "SubjectSubTopics",
                schema: "dbo",
                newName: "SubjectSubTopics",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "SubjectLanguageVariants",
                schema: "dbo",
                newName: "SubjectLanguageVariants",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "StudentTestTest",
                schema: "dbo",
                newName: "StudentTestTest",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "StudentTestStats",
                schema: "dbo",
                newName: "StudentTestStats",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "StudentTests",
                schema: "dbo",
                newName: "StudentTests",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "StudentTestHistories",
                schema: "dbo",
                newName: "StudentTestHistories",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "StudentInvitations",
                schema: "dbo",
                newName: "StudentInvitations",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "StudentAnswerLogs",
                schema: "dbo",
                newName: "StudentAnswerLogs",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "StudentAccountRecoveryAnswers",
                schema: "dbo",
                newName: "StudentAccountRecoveryAnswers",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Student",
                schema: "dbo",
                newName: "Student",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "ScreenAccesses",
                schema: "dbo",
                newName: "ScreenAccesses",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Role",
                schema: "dbo",
                newName: "Role",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "RandomTests",
                schema: "dbo",
                newName: "RandomTests",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "RandomQuestions",
                schema: "dbo",
                newName: "RandomQuestions",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "QuestionTypes",
                schema: "dbo",
                newName: "QuestionTypes",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Questions",
                schema: "dbo",
                newName: "Questions",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Options",
                schema: "dbo",
                newName: "Options",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Notifications",
                schema: "dbo",
                newName: "Notifications",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "MCQAnswers",
                schema: "dbo",
                newName: "MCQAnswers",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Mails",
                schema: "dbo",
                newName: "Mails",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Loggers",
                schema: "dbo",
                newName: "Loggers",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "LanguageVariantQuestions",
                schema: "dbo",
                newName: "LanguageVariantQuestions",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Languages",
                schema: "dbo",
                newName: "Languages",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "GradeLevels",
                schema: "dbo",
                newName: "GradeLevels",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "Comprehensions",
                schema: "dbo",
                newName: "Comprehensions",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "CalculatedResults",
                schema: "dbo",
                newName: "CalculatedResults",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                newName: "AspNetUserTokens",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                newName: "AspNetUserRoles",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                newName: "AspNetUserLogins",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                newName: "AspNetUserClaims",
                newSchema: "bala");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                newName: "AspNetRoleClaims",
                newSchema: "bala");
        }
    }
}
