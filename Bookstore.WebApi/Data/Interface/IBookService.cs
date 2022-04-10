using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookstore.WebApi
{
    public interface IBookService
    {
        List<Book> GetBooks { get; }
        Task<Book> GetBookAsync(string id);
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(string id);
        Task<Book> UpdateBookAsync(Book book);
    }
}
