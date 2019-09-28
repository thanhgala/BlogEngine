using System.Collections.Generic;
using System.Linq;

namespace FrameworkCore.Infrastructure.DAL
{
    public class PaginationSet<T> where T : class
    {
        public PaginationSet()
        {

        }

        public PaginationSet(int pageIndex, int totalPages, int totalCount, int maxPage, List<T> items)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;
            TotalCount = totalCount;
            MaxPage = maxPage;
            Items = items;
        }

        public int PageIndex { set; get; }

        public int Count => Items?.Count() ?? 0;

        public int TotalPages { set; get; }
        public int TotalCount { set; get; }
        public int MaxPage { set; get; }
        public List<T> Items { set; get; }
    }
}
