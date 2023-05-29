using System;
using System.Collections.Generic;

namespace Learning.Entities
{
    public class StudentTest
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TestId { get; set; }
        public int Assigner { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime AssignedOn { get; set; }
        public int StatusId { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<Student> Students { get; }
        public virtual ICollection<Test> Tests { get; set; }
    }
}
