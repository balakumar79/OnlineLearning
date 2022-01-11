using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Utils.Enums
{
   public enum StudentTestStatus
    {
        Initiated=1,
        InProgress=2,
        Submited=3,
        Completed=4,
        Cancelled=5
    }
    public enum SearchFilterEnums
    {
        Testid = 1,
        StudentId = 2,
        Assigner = 3,
        SubjectId = 4,
        StatusId = 5,
        Id = 6,
        BetweenDates = 7
    }
}
