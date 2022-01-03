using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookSearch
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string IBSN { get; set; }
    }
}
