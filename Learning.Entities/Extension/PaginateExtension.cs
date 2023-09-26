using Learning.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Entities.Extension
{
    public static class PaginateExtension
    {
        //used by LINQ to SQL
        public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> source, PaginationQuery pagination)
        {
            pagination.PageCount = source?.Count();
            return source.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
        }
        public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> source, int pageNumber, int pageSize, out int? pageCount)
        {
            pageCount = source?.Count();
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, PaginationQuery pagination)
        {
            return source.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
        }
        public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, int pageNumber,int pageSize,out int? pageCount)
        {
            pageCount = source?.Count();
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
