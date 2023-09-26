using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning.Entities
{
    public class Question
    {
        [Key]
        public int QusID { get; set; }
        public string QuestionName { get; set; }
        public int QuestionTypeId { get; set; }
        public virtual QuestionType QuestionType { get; set; }

        public int TestId { get; set; }
        public virtual Test Test { get; set; }

        [ForeignKey("TestSection")]
        public int? SectionId { get; set; }
        public virtual TestSection TestSection { get; set; }

        [ForeignKey("SubjectTopic")]
        public int? TopicId { get; set; }
        public virtual SubjectTopic SubjectTopic { get; set; }
        [ForeignKey("SubjectSubTopic")]
        public int? SubTopicId { get; set; }
        public virtual SubjectSubTopic SubjectSubTopic { get; set; }
        public int Mark { get; set; }
        public int TestStatusId { get; set; }
        public virtual TestStatus TestStatus { get; set; }

        public bool Deleted { get; set; }
        public string CorrectOption { get; set; }
        public string AnswerExplantion { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public ICollection<Options> Options { get; set; }


    }
}
