using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learning.Entities
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ParentID { get; set; }
        public string UserID { get; set; }
        public string LanguagesKnown { get; set; }
        public int Grade { get; set; }
        public string Institution { get; set; }
        public int MotherTongue { get; set; }
        public string Gender { get; set; }
        public virtual Language LanguageNavigation { get; set; }

    }
}
