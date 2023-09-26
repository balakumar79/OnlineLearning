using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Learning.Entities
{
    public partial class AppDBContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public int userID;

        public AppDBContext() { }
        public AppDBContext(DbContextOptions<AppDBContext> dbContextOptions, IHttpContextAccessor httpContext) : base(dbContextOptions)
        {
            if (httpContext.HttpContext != null)
            {
                if (httpContext.HttpContext.User.Claims.Any())
                {
                    this.userID = Convert.ToInt32(httpContext.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == "nameidentifier")?.Value);
                }
            }
        }

        public virtual DbSet<GradeLevels> GradeLevels { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<StudentTest> StudentTests { get; set; }
        public virtual DbSet<StudentAnswerLog> StudentAnswerLogs { get; set; }
        public virtual DbSet<StudentTestHistory> StudentTestHistories { get; set; }
        public virtual DbSet<CalculatedResult> CalculatedResults { get; set; }
        public virtual DbSet<StudentTestStats> StudentTestStats { get; set; }
        public virtual DbSet<StudentInvitation> StudentInvitations { get; set; }

        public virtual DbSet<Tutor> Tutors { get; set; }
        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<TutorLanguageOfInstruction> TutorLanguageOfInstructions { get; set; }
        public virtual DbSet<TutorGradesTaken> TutorGradesTakens { get; set; }
        public virtual DbSet<TutorEducation> TutorEducations { get; set; }

        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<TestSubject> TestSubjects { get; set; }
        public virtual DbSet<TestSection> TestSections { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Options> Options { get; set; }
        public virtual DbSet<LanguageVariantQuestion> LanguageVariantQuestions { get; set; }
        public virtual DbSet<SubjectLanguageVariant> SubjectLanguageVariants { get; set; }
        public virtual DbSet<RandomTest> RandomTests { get; set; }
        public virtual DbSet<RandomQuestion> RandomQuestions { get; set; }


        public virtual DbSet<MCQAnswer> MCQAnswers { get; set; }
        //public virtual DbSet<GapFillingAnswer> GapFillingAnswers{ get; set; }
        public virtual DbSet<TrueOrFalse> TrueOrFalses { get; set; }
        //public virtual DbSet<Matching> Matchings{ get; set; }
        public virtual DbSet<TestStatus> TestStatuses { get; set; }
        public virtual DbSet<Comprehension> Comprehensions { get; set; }
        public virtual DbSet<SubjectTopic> SubjectTopics { get; set; }

        public virtual DbSet<SubjectSubTopic> SubjectSubTopics { get; set; }

        public virtual DbSet<UserScreenAccess> UserScreensAccess { get; set; }
        public virtual DbSet<ScreenAccess> ScreenAccesses { get; set; }
        public virtual DbSet<StudentAccountRecoveryAnswer> StudentAccountRecoveryAnswers { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Mail> Mails { get; set; }

        public virtual DbSet<Logger> Loggers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("bala");
            base.OnModelCreating(builder);
            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<AppRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            //builder.Entity<Question>().Property<int>("FK_Question_Test_TestID");

            //builder.Entity<Question>().HasOne(t => t.Test).WithMany(t => t.Questions).HasForeignKey("FK_Question_Test_TestID");
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("");
        //    }
        //}
    }
}
