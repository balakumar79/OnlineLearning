using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.UnitOfWork.Interface
{
    public interface IUnitOfWork:IDisposable
    {
        IQuestionRepository QuestionRepository { get; }
        void Commit();
        void RollBack();

       
    }
}
