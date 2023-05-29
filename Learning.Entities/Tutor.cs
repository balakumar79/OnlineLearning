using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning.Entities
{
    public class Tutor
    {
        [Key]
        public int TutorId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
        public string UserName { get; set; }

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
