using Dapper;
using Learning.Admin.Abstract;
using Learning.Entities;
using Learning.Entities.Config;
using Learning.ViewModel.Tutor;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Admin.Repo
{
    public class ManageExamRepo :IManageExamRepo
    {
        readonly AppDBContext _dBContext;
        readonly ConnectionString _connectionString;

        public ManageExamRepo(AppDBContext dBContext,ConnectionString connectionString)
        {
            _connectionString = connectionString;
            _dBContext = dBContext;
        }
          public async Task<DashboardModel> GetDashboardModel(int userid)
        {
            var dashboard = new DashboardModel();
            using (IDbConnection db = new SqlConnection(_connectionString.ConnectionStr))
            {
                db.Open();
               var reader=await db.QueryMultipleAsync("sp_GetDashboard", new { @UserId = userid }, commandType: CommandType.StoredProcedure);
                dashboard = reader.Read<DashboardModel>().FirstOrDefault();
                dashboard.NotificationPartialModels = reader.Read<NotificationPartialModel>().ToList();
                return dashboard;
            }
        }
        public Task<int> UpdateTestStatus(int testid, int statusid)
        {
            var db = _dBContext.Tests.FirstOrDefault(p => p.Id == testid);
            if (db == null)
                return Task.FromResult(0);
            db.TestStatusId = statusid;
            _dBContext.Tests.Update(db);
            return _dBContext.SaveChangesAsync();
        }

        public Task<int> UpdateQuestionStatus(int questionid,int statusid)
        {
            var db = _dBContext.Questions.FirstOrDefault(p => p.QusID == questionid);
            db.TestStatusId = statusid;
            _dBContext.Questions.Update(db);
            return _dBContext.SaveChangesAsync();
        }

        public List<TestStatus> GetAllStatuses()
        {
            return _dBContext.TestStatuses.ToList();
        }
    }
}
