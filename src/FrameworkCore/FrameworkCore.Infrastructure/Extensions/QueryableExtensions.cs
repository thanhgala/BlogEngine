using System;
using System.Linq;
using FrameworkCore.Infrastructure.DAL;

namespace FrameworkCore.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static PaginationSet<T> ToPaginatedList<T>(
           this IQueryable<T> query, int pageIndex, int pageSize, int maxPage) where T : class
        {
            var totalCount = query.Count();

            var data = query.Skip((pageIndex - pageSize) * pageSize).Take(pageSize);

            var collection = !data.Any()
                ? null : data.ToList();

            var totalPage = (int)Math.Ceiling((double)totalCount / pageSize);

            return new PaginationSet<T>(pageIndex, totalPage, totalCount, maxPage, collection);
        }
    }
}
