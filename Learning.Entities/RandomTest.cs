using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Entities
{
    public class RandomTest
    {
        [Key]
        public int TestId { get; set; }
        public int? TopicId { get; set; }
        public int? SubTopicId { get; set; }
        public virtual Test Test { get; set; }
    }
}
