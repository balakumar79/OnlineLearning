using System.Collections.Generic;

namespace Learning.Student.ViewModel
{
    public class TestSubjectViewModel
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        //public List<GradeLevels> GradeLevels{ get; set; }
    }
    public class Languages
    {
        public int LangId { get; set; }
        public string Language { get; set; }
        public List<Grades> Grades { get; set; }
    }
    public class Grades
    {
        public int GradeID { get; set; }
        public string Grade { get; set; }
        public List<TestSubjectViewModel> TestSubjects { get; set; }
    }
}
