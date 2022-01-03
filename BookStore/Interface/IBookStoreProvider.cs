using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Interface
{
    public interface IBookStoreProvider
    {
        Task<(bool IsSucess, IEnumerable<Models.Book> Books, string ErrorMessage)> GetBookSearchAsync(Models.BookSearch searchParams);

        Task<(bool IsSucess, Models.Book Book, string ErrorMessage)> GetBookAsync(int bookId);

        Task<(bool IsSucess, IEnumerable<Models.BookRequest> BookRequests, string ErrorMessage)> GetBookRequestsAsync(int bookId);

        Task<(bool IsSucess, IEnumerable<Models.BookRequest> BookRequests, string ErrorMessage)> GetCurrentBookRequestsByCustomerAsync(int customerId);

        Task<(bool IsSucess, IEnumerable<Models.BookRequest> BookRequests, string ErrorMessage)> GetPreviousBookRequestsByCustomerAsync(int customerId);

        Task<(bool IsSucess, int BookId, string ErrorMessage)> AddBookAsync(Models.Book book);

        Task<(bool IsSucess, string ErrorMessage)> RemoveBookAsync(int BookId);
        Task<(bool IsSucess, int BookRequestId, string ErrorMessage)> RequestBookAsync(int customerId, int bookId, int qty);
        Task<(bool IsSucess, int BookReturnId, string ErrorMessage)> ReturnBookAsync(int customerId, int bookId, int qty);
        Task<(bool IsSucess, IEnumerable<Models.Book> Books, string ErrorMessage)> GetCustomerBookRequestsAsync(int customerId);
    }
}
