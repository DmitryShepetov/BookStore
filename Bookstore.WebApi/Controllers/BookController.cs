using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookstore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookServices;
        public BookController(IBookService bookServices)
        {
            this.bookServices = bookServices;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok(bookServices.GetBooks);
        }

        [HttpGet("{id}", Name ="GetBook")]
        public async Task<IActionResult> GetBook(string id)
        {
            return Ok(await bookServices.GetBookAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddBook(Book book)
        {
            await bookServices.AddBookAsync(book);
            return CreatedAtRoute("GetBook", new { id = book.id }, book);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            await bookServices.DeleteBookAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateBook(Book book)
        {
            return Ok(await bookServices.UpdateBookAsync(book));
        }
    }
}
