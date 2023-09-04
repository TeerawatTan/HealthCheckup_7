using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Dtos
{
    public class ResultResponse
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }

    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PageginationResultResponse<TModel>
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IList<TModel> Items { get; set; }

        public PageginationResultResponse()
        {
            Items = new List<TModel>();
        }
    }
}
