using BookStore.Interface;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/store")]
    public class BookStoreController : ControllerBase
    {
        private readonly IBookStoreProvider bookStoreProvider;
        public BookStoreController(IBookStoreProvider bookStoreProvider)
        {
            this.bookStoreProvider = bookStoreProvider;
        }

        [Route("api/store/AddBook")]
        [HttpPost]
        public async Task<IActionResult> AddBookAsync([FromBody] Book book)
        {
            var result = await bookStoreProvider.AddBookAsync(book);
            if (result.IsSucess)
            {
                return Ok(result.BookId);
            }
            return NotFound();
        }

        [Route("api/store/GetBook")]
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookAsync(int bookId)
        {
            var result = await bookStoreProvider.GetBookAsync(bookId);
            if (result.IsSucess)
            {
                return Ok(result.Book);
            }
            return NotFound();
        }

        [Route("api/store/GetBookSearch")]
        [HttpPost]
        public async Task<IActionResult> GetBookSearchAsync([FromBody] BookSearch bookSearch)
        {
            var result = await bookStoreProvider.GetBookSearchAsync(bookSearch);
            if (result.IsSucess)
            {
                return Ok(result.Books);
            }
            return NotFound();
        }

        [Route("api/store/GetBookRequests")]
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookRequestsAsync(int bookId)
        {
            var result = await bookStoreProvider.GetBookRequestsAsync(bookId);
            if (result.IsSucess)
            {
                return Ok(result.BookRequests);
            }
            return NotFound();
        }

        [Route("api/store/GetCurrentBookRequestsByCustomer")]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCurrentBookRequestsByCustomerAsync(int customerId)
        {
            var result = await bookStoreProvider.GetCurrentBookRequestsByCustomerAsync(customerId);
            if (result.IsSucess)
            {
                return Ok(result.BookRequests);
            }
            return NotFound();
        }

        [Route("api/store/GetCustomerBookRequests")]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerBookRequestsAsync(int customerId)
        {
            var result = await bookStoreProvider.GetCustomerBookRequestsAsync(customerId);
            if (result.IsSucess)
            {
                return Ok(result.Books);
            }
            return NotFound();
        }

        [Route("api/store/GetPreviousBookRequestsByCustomer")]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetPreviousBookRequestsByCustomerAsync(int customerId)
        {
            var result = await bookStoreProvider.GetPreviousBookRequestsByCustomerAsync(customerId);
            if (result.IsSucess)
            {
                return Ok(result.BookRequests);
            }
            return NotFound();
        }

        [Route("RemoveBook")]
        [HttpPost("{bookId}")]
        public async Task<IActionResult> RemoveBookAsync(int bookId)
        {
            var result = await bookStoreProvider.RemoveBookAsync(bookId);
            if (result.IsSucess)
            {
                return Ok(result.IsSucess);
            }
            return NotFound();
        }

        [Route("RequestBook")]
        [HttpPost("{customerId}/{bookId}/{qty}")]
        public async Task<IActionResult> RequestBookAsync(int customerId, int bookId, int qty)
        {
            var result = await bookStoreProvider.RequestBookAsync(customerId, bookId, qty);
            if (result.IsSucess)
            {
                return Ok(result.BookRequestId);
            }
            return NotFound();
        }

        [Route("ReturnBook")]
        [HttpPost("{customerId}/{bookId}/{qty}")]
        public async Task<IActionResult> ReturnBookAsync(int customerId, int bookId, int qty)
        {
            var result = await bookStoreProvider.ReturnBookAsync(customerId, bookId, qty);
            if (result.IsSucess)
            {
                return Ok(result.BookReturnId);
            }
            return NotFound();
        }
    }
}
