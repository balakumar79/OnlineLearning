using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learning.Entities
{
    [Table("ScreenAccesses")]
   public class ScreenAccess
    {
        public int Id { get; set; }
        public int RoleID { get; set; }
        public string ScreenPermission { get; set; }
    }
}
