using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.TeacherServ.Viewmodel
{
   public class StudentModel
    {
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentGender { get; set; }
        public string StudentUserName { get; set; }
        public int UserId { get; set; }
        public string Grade { get; set; }
        public string Institution { get; set; }
        public string StudentDistrict { get; set; }
        public string LanguageKnown { get; set; }
        public int MotherTongue { get; set; }
    }
}
