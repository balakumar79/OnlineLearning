using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.Abstract
{
    public interface IStudentTestService
    {
        List<StudentTestViewModel> GetTestByUserID(int userid);
    }
}
