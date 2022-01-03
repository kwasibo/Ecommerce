using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookReturn
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int BookId { get; set; }
        public DateTime ReturnDate { get; set; }
        public int Qty { get; set; }
    }
}
