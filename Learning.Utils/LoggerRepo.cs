using Learning.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Utils
{
   public class LoggerRepo
    {
        readonly AppDBContext _dBContext;
        public LoggerRepo(AppDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public void InsertLogger(Exception ex)
        {
            
            var logger = new Logger
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
        public void InsertLogger(Logger logger)
        {
            logger.CreatedAt = DateTime.Now;
            _dBContext.Loggers.Add(logger);
            _dBContext.SaveChanges();
        }
    }
}
