using Learning.Entities;
using Learning.Tutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student
{
   public class StudentTestViewModel: TestViewModel
    {
        
        public int TestId { get; set; }
        public int StudentId { get; set; }
        public int Assigner { get; set; }
        public DateTime AssignedOn { get; set; }
        public bool Active { get; set; }
        public StudentTestStats  StudentTestStats { get; set; }
        public IEnumerable<StudentTestHistory> StudentTestHistories{ get; set; }
    }
    public class StudentTestStatusPartialModel
    {
        public int StudentTestId { get; set; }
        public int StatusId { get; set; }
    }
}
