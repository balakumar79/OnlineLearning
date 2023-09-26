using Learning.Entities;
using Learning.UnitOfWork.Interface;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.UnitOfWork
{
    public class UnitOfWork :IUnitOfWork,IDisposable
    {
        private readonly AppDBContext _dBContext;
        public IQuestionRepository QuestionRepository { get; }
        public UnitOfWork(AppDBContext dBContext,IQuestionRepository questionRepository)
        {
              _dBContext= dBContext;
            QuestionRepository= questionRepository;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dBContext.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Commit()
        {
            _dBContext.SaveChanges();
        }
        public void RollBack()
        {
            foreach (var entry in _dBContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case Microsoft.EntityFrameworkCore.EntityState.Added:
                        entry.State=Microsoft.EntityFrameworkCore.EntityState.Detached; break;
                }
            }
        }
      

    }
}
