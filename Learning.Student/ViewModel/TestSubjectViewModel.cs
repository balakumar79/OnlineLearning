using Learning.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.ViewModel
{
   public class TestSubjectViewModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public List<GradeLevels> GradeLevels{ get; set; }
    }
}
