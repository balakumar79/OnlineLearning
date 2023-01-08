using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.ViewModel.Tutor
{
   public class DashboardModel
    {
        public int AvailableTests { get; set; }
        public int RegisteredStudents { get; set; }
        public int NewRegistration { get; set; }
        public int TestTakenByStudentToday { get; set; }
        public DateTime LastAttemptedTestOn { get; set; }
        public string TutorName { get; set; }
        public List<NotificationPartialModel> NotificationPartialModels { get; set; }
    }
    public partial class NotificationPartialModel
    {
        public string DateTime { get; set; }
        public string Message { get; set; }
    }
}
