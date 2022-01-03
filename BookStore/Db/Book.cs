using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Db
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IBSN { get; set; }
        public string Author { get; set; }

        public string Publisher { get; set; }

        public int NoOfPages { get; set; }

        public bool IsDeleted { get; set; }
    }
}
