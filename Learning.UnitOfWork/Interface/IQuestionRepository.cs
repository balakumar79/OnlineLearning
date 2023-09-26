using Learning.Entities;
using Learning.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.UnitOfWork.Interface
{
    public interface IQuestionRepository :IRepository<Question>
    {
        IEnumerable<SubjectTopic> GetSubjectTopicsBySubjectId(int subjectId);
         IEnumerable<Question> GetQuestionsByTopidId(int topicId);
        IEnumerable<SubjectSubTopic> GetSubTopicByTopicId(int topicId);
        IEnumerable<Question> GetQuestionsBySubjectSubTopicId(int subTopicId);
    }
}
