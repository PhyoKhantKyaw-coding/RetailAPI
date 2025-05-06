using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs
{

    public class PaginationDTO
    {
        public int page { get; set; } 
        public int pageSize { get; set; }
    }
    public class ConditionPaginationDTO<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}

