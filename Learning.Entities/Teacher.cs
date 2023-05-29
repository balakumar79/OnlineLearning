using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning.Entities
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }
    }
}
