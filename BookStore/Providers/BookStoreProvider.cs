using AutoMapper;
using BookStore.Db;
using BookStore.Interface;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BookStore.Providers
{
    public class BookStoreProvider : IBookStoreProvider
    {
        private readonly BookStoreDbContext dbContext;
        private readonly ILogger<BookStoreProvider> logger;
        private readonly IMapper mapper;

        public BookStoreProvider(BookStoreDbContext dbContext, ILogger<BookStoreProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<(bool IsSucess, int BookId, string ErrorMessage)> AddBookAsync(Models.Book book)
        {
            try
            {
                var bookNew = new Db.Book
                {
                    Title = book.Title,
                    Description = book.Description,
                    IBSN = book.IBSN,
                    NoOfPages = book.NoOfPages,
                    Author = book.Author,
                    Publisher = book.Publisher
                };
                await dbContext.Books.AddAsync(bookNew);

                return (true, bookNew.Id, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, 0, ex.Message);
            }
        }

        public async Task<(bool IsSucess, Models.Book Book, string ErrorMessage)> GetBookAsync(int bookId)
        {
            try
            {
                var book = await dbContext.Books.Where(b => b.Id == bookId).FirstOrDefaultAsync();

                if (book != null)
                {                
                    var result = mapper.Map<Db.Book, Models.Book>(book);

                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSucess, IEnumerable<Models.BookRequest> BookRequests, string ErrorMessage)> GetBookRequestsAsync(int bookId)
        {
            try
            {
                var bookRequests = await dbContext.BookRequests.Where(r=> r.BookId == bookId).ToListAsync();

                if (bookRequests != null && bookRequests.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.BookRequest>, IEnumerable<Models.BookRequest>>(bookRequests);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSucess, IEnumerable<Models.Book> Books, string ErrorMessage)> GetBookSearchAsync(BookSearch searchParams)
        {
            try
            {
                var books = await (from b in dbContext.Books
                                    where (searchParams.Title == "" || b.Title.Contains(searchParams.Title))
                                    && (searchParams.Title == "" || b.Description.Contains(searchParams.Title))
                                    && (searchParams.IBSN == "" || b.Description.Contains(searchParams.IBSN))
                                    && (searchParams.Author == "" || b.Author.Contains(searchParams.Author))
                                    select b
                                    ).ToListAsync();
                    

                if (books != null && books.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Book>, IEnumerable<Models.Book>>(books);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSucess, IEnumerable<Models.BookRequest> BookRequests, string ErrorMessage)> GetCurrentBookRequestsByCustomerAsync(int customerId)
        {
            try
            {
                var bookRequests = await dbContext.BookRequests.Where(r => r.UserId == customerId && !r.RequestCompleted).ToListAsync();

                if (bookRequests != null && bookRequests.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.BookRequest>, IEnumerable<Models.BookRequest>>(bookRequests);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSucess, IEnumerable<Models.Book> Books, string ErrorMessage)> GetCustomerBookRequestsAsync(int customerId)
        {
            try
            {
                var books = await (from b in dbContext.Books
                                   join r in dbContext.BookRequests on b.Id equals r.BookId
                                   where r.UserId == customerId
                                   select b
                                    ).ToListAsync();

                if (books != null && books.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Book>, IEnumerable<Models.Book>>(books);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSucess, IEnumerable<Models.BookRequest> BookRequests, string ErrorMessage)> GetPreviousBookRequestsByCustomerAsync(int customerId)
        {
            try
            {
                var bookRequests = await dbContext.BookRequests.Where(r => r.UserId == customerId && r.RequestCompleted).ToListAsync();

                if (bookRequests != null && bookRequests.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.BookRequest>, IEnumerable<Models.BookRequest>>(bookRequests);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSucess, string ErrorMessage)> RemoveBookAsync(int bookId)
        {
            try
            {
                var book = await dbContext.Books.Where(b => b.Id == bookId).FirstOrDefaultAsync();

                if (book != null)
                {
                    book.IsDeleted = true;
                    await dbContext.SaveChangesAsync();

                    return (true,  null);
                }
                return (false, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false,  ex.Message);
            }
        }

        public async Task<(bool IsSucess, int BookRequestId, string ErrorMessage)> RequestBookAsync(int customerId, int bookId, int qty)
        {
            try
            {
                Db.BookRequest bookRequest = new Db.BookRequest
                {
                    UserId = customerId,
                    BookId = bookId,
                    Qty = qty,
                    RequestDate = DateTime.Now,
                    RequestCompleted = false
                };
                await dbContext.AddAsync(bookRequest);
                await dbContext.SaveChangesAsync();

                return (true, bookRequest.Id, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, 0, ex.Message);
            }
        }

        public async Task<(bool IsSucess, int BookReturnId, string ErrorMessage)> ReturnBookAsync(int customerId, int bookId, int qty)
        {
            try
            {
                Db.BookReturn bookReturn = new Db.BookReturn
                {
                    UserId = customerId,
                    BookId = bookId,
                    ReturnDate = DateTime.Now,
                    Qty = qty
                };
                await dbContext.AddAsync(bookReturn);

                var inventory = await dbContext.Inventories.Where(i => i.BookId == bookId).FirstOrDefaultAsync();
                inventory.Qty -= qty; 

                await dbContext.SaveChangesAsync();

                return (true, bookReturn.Id, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, 0, ex.Message);
            }
        }
    }
}
