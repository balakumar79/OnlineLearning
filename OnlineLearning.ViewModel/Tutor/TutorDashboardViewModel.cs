using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.ViewModel.Tutor
{
   public class TutorDashboardViewModel
    {
        public int TestAvailable { get; set; }
        public int TestPublished { get; set; }
        public int AssignedTest { get; set; }
        public int TestCompleted { get; set; }
    }
}
