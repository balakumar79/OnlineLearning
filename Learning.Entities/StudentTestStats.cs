using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class StudentTestStats
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public decimal MaximumMarkScored { get; set; }
        public decimal MinimumMarkScored { get; set; }
        public decimal AverageMarkScored { get; set; }
        /// <summary>
        /// Number of attempts completed by the students registered for the exam.
        /// </summary>
        public int NumberOfAttempts { get; set; }
        /// <summary>
        /// Total registered student count for the exam
        /// </summary>
        public int TotalRegistration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
