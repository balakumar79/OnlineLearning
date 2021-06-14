using Learning.Entities;
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
        public AppDBContext(DbContextOptions<AppDBContext> dbContextOptions,IHttpContextAccessor httpContext):base(dbContextOptions) {
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
        public virtual DbSet<MCQAnswer> MCQAnswers{ get; set; }
        public virtual DbSet<TestStatus> TestStatuses { get; set; }


        public virtual DbSet<UserScreenAccess> UserScreensAccess { get; set; }
        public virtual DbSet<ScreenAccess> ScreenAccesses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");
            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<AppRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
