using Learning.Student.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.Services
{
   public class StudentTestService:IStudentTestService
    {
        readonly IStudentTestRepo _studentTestRepo;
        public StudentTestService(IStudentTestRepo studentTestRepo)
        {
            _studentTestRepo = studentTestRepo;
        }
       public List<StudentTestViewModel> GetTestByUserID(int userid)
        {
            return _studentTestRepo.GetTestByUserID(userid);
        }

    }
}
