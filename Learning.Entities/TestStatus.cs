using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Entities
{
   public class TestStatus
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
    }
}
