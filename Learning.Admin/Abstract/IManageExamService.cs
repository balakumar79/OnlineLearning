using Learning.Entities;
using Learning.ViewModel.Tutor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Abstract
{
    public interface IManageExamService
    {
        Task<DashboardModel> GetDashboardModel(int userid);
        Task<int> UpdateTestStatus(int testid, int statusid);
        Task<int> UpdateQuestionStatus(int questionid, int statusid);
        Task<bool> DeleteQuestion(int questionid);
        Task<bool> DeleteTest(int testid);
        List<TestStatus> GetAllStatuses();
    }
}
