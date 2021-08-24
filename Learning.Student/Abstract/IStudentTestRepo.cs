using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.Abstract
{
   public interface IStudentTestRepo
    {
        List<StudentTestViewModel> GetTestByUserID(int userid);

    }
}
