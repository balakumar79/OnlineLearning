using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.TeacherServ.Viewmodel
{
   public class StudentSearchRequest
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public List<int>? Grades { get; set; } 
        public List<string>? Districts { get; set; }
        public string Gender { get; set; } 
        public string UserName { get; set; }
        public List<string>? Institution { get; set; }
    }
}
