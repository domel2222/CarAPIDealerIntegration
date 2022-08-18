using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Extensions.Pagination
{
    public class Paginator<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
        public int TotalItems {get;set;}


        public Paginator(IEnumerable<T> items, int totalItems, int pageSize, int pageNumber)
        {
            Items = items;
            TotalPages = (int)Math.Ceiling(totalItems/(double)pageSize);
            TotalItems = totalItems;
            StartPoint = pageSize * pageNumber - pageSize + 1;
            EndPoint = StartPoint + pageSize - 1;

        }
    }
}
