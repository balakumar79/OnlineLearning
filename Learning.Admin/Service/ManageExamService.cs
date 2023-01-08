using Learning.Admin.Abstract;
using Learning.Entities;
using Learning.ViewModel.Tutor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Service
{
   public class ManageExamService:IManageExamService
    {
        readonly IManageExamRepo _manageExamRepo;

        public Task<DashboardModel> GetDashboardModel(int userid)
        {
            return _manageExamRepo.GetDashboardModel(userid);
        }
        public ManageExamService(IManageExamRepo manageExamRepo)
        {
            _manageExamRepo = manageExamRepo;
        }
       public async Task<int> UpdateTestStatus(int testid, int statusid)
        {
            return  await _manageExamRepo.UpdateTestStatus(testid, statusid);
        }
       public Task<int> UpdateQuestionStatus(int questionid, int statusid)
        {
            return _manageExamRepo.UpdateQuestionStatus(questionid, statusid);
        }
       public List<TestStatus> GetAllStatuses()
        {
            return _manageExamRepo.GetAllStatuses();
        }
    }
}
