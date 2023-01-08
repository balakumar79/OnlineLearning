using Learning.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.LogMe.Repo
{
   public class LoggerRepo:ILoggerRepo
    {
        readonly AppDBContext _dBContext;
        public LoggerRepo(AppDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public void InsertLogger(Exception ex)
        {

            var logger = new Entities.Logger
            {
                Link = ex.StackTrace,
                Type = ex.GetType().Name,
                CreatedAt = DateTime.Now,
                Description = ex.ToString(),
                Message =
                ex.InnerException == null ? ex.Message : ex.InnerException.Message
            };
            _dBContext.Loggers.Add(logger);
            _dBContext.SaveChanges();
        }
        public void InsertLogger(string type,string message,string description,string ? link=null)
        {
            var logger = new Entities.Logger
            {
                Type = type,
                Message = message,
                Description = description,
                Link = link
            };
            logger.CreatedAt = DateTime.Now;
            _dBContext.Loggers.Add(logger);
            _dBContext.SaveChanges();
        }
    }
}
