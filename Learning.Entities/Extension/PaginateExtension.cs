using Learning.Entities.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Learning.Entities.Extension
{
    public static class PaginateExtension
    {
        //used by LINQ to SQL
        public static PaginationResult<TSource> Paginate<TSource>(this IQueryable<TSource> source, PaginationQuery pagination) where TSource : PaginationResult<TSource>
        {
            var paginateResult = new PaginationResult<TSource>
            {
                recordsTotal = source?.Count(),

                data = source.FilterByProperties(pagination.SearchString)
            };

            paginateResult.data = source.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
            paginateResult.recordsFiltered = paginateResult.data.Count();
            paginateResult.draw = pagination.Draw;

            return paginateResult;
        }
        public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> source, int pageNumber, int pageSize, out int? pageCount)
        {
            pageCount = source?.Count();
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static PaginationResult<TSource> Paginate<TSource>(this IEnumerable<TSource> source, PaginationQuery pagination) where TSource : class
        {
            var recordFiltered = source.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
            var paginateResult = new PaginationResult<TSource>
            {
                data = recordFiltered,
                recordsFiltered = source?.Count()
            };
            paginateResult.recordsTotal = paginateResult.data.Count();

            if (!string.IsNullOrEmpty(pagination.Order))
            {
                var order = JsonConvert.DeserializeObject<List<DataTablesOrder>>(pagination.Order);
                var column = JsonConvert.DeserializeObject<List<DataTablesColumn>>(pagination.Columns);

                if (order.Count > 0 && column.Count > 0)
                {
                    // Get the column name dynamically based on the column index
                    string columnName = column[order[0].Column].Data;

                    // Determine the sorting direction
                    bool isAscending = order[0].Dir == "asc";
                    //Order the data dynamically based on the column name and sorting direction
                    paginateResult.data = isAscending ?
                        paginateResult.data.OrderBy(s => s.GetType().GetProperty(columnName)?.GetValue(s, null)).ToList() :
                        paginateResult.data.OrderByDescending(s => s.GetType().GetProperty(columnName)?.GetValue(s, null)).ToList();
                }

            }
            paginateResult.draw = pagination.Draw;
            return paginateResult;
        }

        public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, int pageNumber, int pageSize, out int? pageCount)
        {
            pageCount = source?.Count();
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

    }
}
