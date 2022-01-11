using Learning.Admin.Abstract;
using Learning.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Repo
{
    public class ManageExamRepo :IManageExamRepo
    {
        readonly AppDBContext _dBContext;

        public ManageExamRepo(AppDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public Task<int> UpdateTestStatus(int testid, int statusid)
        {
            var db = _dBContext.Tests.FirstOrDefault(p => p.Id == testid);
            if (db == null)
                return Task.FromResult(0);
            db.StatusID = statusid;
            _dBContext.Tests.Update(db);
            return _dBContext.SaveChangesAsync();
        }

        public Task<int> UpdateQuestionStatus(int questionid,int statusid)
        {
            var db = _dBContext.Questions.FirstOrDefault(p => p.QusID == questionid);
            db.StatusId = statusid;
            _dBContext.Questions.Update(db);
            return _dBContext.SaveChangesAsync();
        }

        public List<TestStatus> GetAllStatuses()
        {
            return _dBContext.TestStatuses.ToList();
        }
    }
}
