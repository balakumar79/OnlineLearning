using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learning.Entities
{
   public class SubjectLanguageVariant
    {
        public int Id { get; set; }
        [ForeignKey(nameof(TestSubject))]
        public int SubjectId { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public virtual TestSubject TestSubject { get; set; }
    }
}
