using Learning.Entities;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student
{
   public class StudentTestViewModel: TestViewModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int StudentId { get; set; }
        public int Assigner { get; set; }
        public DateTime AssignedOn { get; set; }
        public IEnumerable<StudentTestHistory> StudentTestHistories{ get; set; }
    }
}
