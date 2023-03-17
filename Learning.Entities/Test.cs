using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Learning.Entities
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string TestDescription { get; set; }
        public int GradeLevelsId { get; set; }
        public virtual GradeLevels GradeLevels { get; set; }
        public int TestSubjectId { get; set; }
        public virtual TestSubject TestSubject { get; set; }

        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PassingMark { get; set; }
        public int TestStatusId { get; set; }
        public virtual TestStatus TestStatus { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }
        public bool IsPublished { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Question> Questions { get; set; }


    }
}
