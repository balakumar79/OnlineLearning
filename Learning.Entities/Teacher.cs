using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Entities
{
   public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        public AppUser Id { get; set; }
    }
}
