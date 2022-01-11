using Learning.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.ViewModel
{
   public class TestGradeViewModel
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public List<TestSubject> TestSubjects{ get; set; }
    }
}
