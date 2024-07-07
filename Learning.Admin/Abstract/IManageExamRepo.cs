using Learning.Entities;
using Learning.ViewModel.Tutor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learning.Admin.Abstract
{
    public interface IManageExamRepo
    {
        Task<int> UpdateTestStatus(int testid, int statusid);
        Task<int> UpdateQuestionStatus(int questionid, int statusid);
        List<TestStatus> GetAllStatuses();
        Task<bool> DeleteTest(int testid);
        Task<bool> DeleteQuestion(int questionid);
        Task<bool> IsExamExists(string title, int id, int tutorId);
        Task<DashboardModel> GetDashboardModel(int userid);
    }
}
