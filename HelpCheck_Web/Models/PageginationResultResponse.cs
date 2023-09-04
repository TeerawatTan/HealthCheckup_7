using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class PageginationResultResponse<TModel>
    {
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int totalItems { get; set; }
        public int totalPages { get; set; }
        public IList<TModel> Items { get; set; }

        public PageginationResultResponse()
        {
            Items = new List<TModel>();
        }


    }
}
