using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities.Domain
{
    public class GetTestRequestModel
    {
        public int? TestId { get; set; }
        public int? GradeId { get; set; }
        public int? SubjectId { get; set; }
        public int? LanguageId { get; set; }
        public int? StudentId { get; set; }
        public PaginationQuery PaginationQuery { get; set; }
        public bool IsTutorviewOnly { get; set; }
    }
}
