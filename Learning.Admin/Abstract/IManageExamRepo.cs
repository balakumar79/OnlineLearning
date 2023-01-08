using Learning.Entities;
using Learning.ViewModel.Tutor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Abstract
{
   public interface IManageExamRepo
    {
        Task<int> UpdateTestStatus(int testid, int statusid);
        Task<int> UpdateQuestionStatus(int questionid, int statusid);
        List<TestStatus> GetAllStatuses();

        Task<DashboardModel> GetDashboardModel(int userid);
    }
}
