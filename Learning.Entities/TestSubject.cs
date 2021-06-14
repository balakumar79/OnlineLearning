using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Entities
{
   public class TestSubject
    {
        [Key]
        public int Id { get; set; }
        public string SubjectName { get; set; }
    }
}
