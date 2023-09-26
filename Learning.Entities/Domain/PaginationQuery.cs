using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities.Domain
{
    public class PaginationQuery
    {
        public PaginationQuery()
        {

        }

        public PaginationQuery(int pageNumer, int pageSize, int? pageCount = 0)
        {
            PageNumber = pageNumer;
            PageSize = pageSize;
            PageCount = pageCount;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? PageCount { get; set; }
    }
}
