using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DTO
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PageResult(List<T> items,int totalCount,int pageSize,int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            int from = pageSize * (pageNumber - 1) + 1;
            ItemsFrom =from;
            ItemsTo =from+pageSize-1;
            TotalPages = (int)Math.Ceiling(totalCount/(double)pageSize);
        }
    }
}
