using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learning.Entities
{
        
   public class UserScreenAccess
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string Screen { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        [ForeignKey("FK_UserID")]
        public virtual AppUser User { get; set; }
    }
}
