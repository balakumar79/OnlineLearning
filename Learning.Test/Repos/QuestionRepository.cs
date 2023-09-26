using Learning.Entities;
using Learning.UnitOfWork.Interface;
using Learning.UnitOfWork;
using Learning.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Learning.UnitOfWork.Repos
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDBContext dBContext):base(dBContext) 
        {

        }
        public IEnumerable<SubjectTopic> GetSubjectTopicsBySubjectId(int subjectId)
        {
            return _dBContext.SubjectTopics.Where(s=>s.TestSubjectId==subjectId);
        }
        /// <summary>
        /// Get Questions by TopicId
        /// </summary>
        /// <param name="topicId">TopicId</param>
        /// <returns>IEnumerable<Qustion></Qustion></returns>
        public IEnumerable<Question> GetQuestionsByTopidId(int topicId)
        {
            return _dBContext.Questions.Include(s => s.SubjectTopic).Where(s => s.TopicId == topicId);
        }
        public IEnumerable<SubjectSubTopic> GetSubTopicByTopicId(int topicId)
        {
            return _dBContext.SubjectSubTopics.Where(s=>s.SubjectTopicId== topicId);
        }
        public IEnumerable<Question> GetQuestionsBySubjectSubTopicId(int subTopicId)
        {
            return _dBContext.Questions.Where(s => s.SubTopicId == subTopicId);
        }
       
    }
}
