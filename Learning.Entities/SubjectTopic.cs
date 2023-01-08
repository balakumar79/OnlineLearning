using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learning.Entities
{
    public class SubjectTopic
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int TestSubjectId { get; set; }
        public virtual TestSubject TestSubject { get; set; }
        public virtual ICollection<SubjectSubTopic> SubjectSubTopics { get; set; }
    }
}
