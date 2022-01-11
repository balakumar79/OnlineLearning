using Learning.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Abstract
{
   public interface IManageExamService
    {
        Task<int> UpdateTestStatus(int testid, int statusid);
        Task<int> UpdateQuestionStatus(int questionid, int statusid);
        List<TestStatus> GetAllStatuses();
    }
}
