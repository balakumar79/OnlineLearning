using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learning.Entities
{
   public class Tutor
    {
        [Key]
        public int TutorId { get; set; }
        [ForeignKey("FK_UserID")]
        public int UserId { get; set; }

        public int TutorType { get; set; }
        public string Organization { get; set; }
        public int PreferredLanguage { get; set; }
        public string Educations { get; set; }
        public string HearAbout { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
