using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
    public class SubjectSubTopic
    {
        public int Id { get; set; }
        public string SubTopic { get; set; }
        public int SubjectTopicId { get; set; }
        public virtual SubjectTopic SubjectTopic { get; set; }

    }
}
