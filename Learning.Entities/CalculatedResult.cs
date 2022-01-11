using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class CalculatedResult
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public StudentTest StudentTest { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public string CalculatedResults { get; set; }
    }
}
