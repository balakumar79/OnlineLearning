using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities.Domain
{
    public class PaginationQuery
    {
        public PaginationQuery()
        {
            PageNumber = PageNumber == 0 ? 1 : PageNumber;
            PageSize = PageSize == 0 ? 10 : PageSize;
        }

        public PaginationQuery(int pageNumer, int pageSize)
        {
            PageNumber = pageNumer == 0 ? 1 : pageNumer;
            PageSize = pageSize == 0 ? 10 : pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Draw { get; set; }
        public string SearchString { get; set; }
        public string Order { get; set; }
        public string Columns { get; set; }
    }
}
