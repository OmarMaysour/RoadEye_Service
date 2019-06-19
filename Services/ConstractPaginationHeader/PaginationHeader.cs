using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadEye_Service.Services.ConstractPaginationHeader
{
    public class PaginationHeader
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string PreviousPageLink { get; set; }
        public string NextPageLink { get; set; }
    }
}
