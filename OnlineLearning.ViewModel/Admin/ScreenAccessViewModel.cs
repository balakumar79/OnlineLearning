using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.ViewModel.Admin
{
   public class ScreenAccessViewModel
    {
        public string ScreenName { get; set; }
        public Enums.Roles Roles{ get; set; }
        public bool IsSubscribed { get; set; }
    }
}
