using System.Collections.Generic;

namespace HelpCheck_Web.Models
{
    public class PageResult<TModel>
    {
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int totalItems { get; set; }
        public int totalPages { get; set; }
        public IList<TModel> Items { get; set; }

        public PageResult()
        {
            Items = new List<TModel>();
        }
    }
}
