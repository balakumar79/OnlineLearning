using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Entities.Domain
{
    public class PaginationResult<T> where T : class
    {
        public int draw { get; set; }
        public int? recordsTotal { get; set; }
        public IEnumerable<T> data { get; set; }
        public int? recordsFiltered { get; set; }
        public DataTablesSearch Search { get; set; }
        public DataTablesOrder[] DataTablesOrders { get; set; }
        public List<DataTablesColumn> Columns { get; set; }
    }

    public class DataTablesSearch
    {
        public string Value { get; set; }
    }

    public class DataTablesOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class DataTablesColumn
    {
        public string Data { get; set; }
    }
}

