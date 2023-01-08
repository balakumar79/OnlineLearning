using Microsoft.AspNetCore.Identity;
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
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LanguagesKnown { get; set; }
        public int Grade { get; set; }
        public string Institution { get; set; }
        public string StudentDistrict { get; set; }
        public int MotherTongue { get; set; }
        public int GenderId { get; set; }
        public int RoleId { get; set; }
        public virtual Language LanguageNavigation { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool Deleted { get; set; }

        private GenderEnum _gender;
        public GenderEnum Gender { get=>_gender=(GenderEnum) GenderId; set=>_gender=value; }

    }
}
