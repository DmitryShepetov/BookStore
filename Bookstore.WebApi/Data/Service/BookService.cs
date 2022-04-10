using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.WebApi
{
    public class BookService : IBookService
    {
        private readonly AppDbContext appDbContext;
        public BookService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddBookAsync(Book book)
        {
            appDbContext.Book.Add(book);
            await appDbContext.SaveChangesAsync();

        }
        public async Task DeleteBookAsync(string id) 
        {
            Book Book = (Book)appDbContext.Book.Where(x => x.id == id);
            appDbContext.Book.Remove(Book);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<Book> GetBookAsync(string id) => await appDbContext.Book.FirstOrDefaultAsync(book => book.id == id);

        public List<Book> GetBooks => appDbContext.Book.ToList();


        public async Task<Book> UpdateBookAsync(Book book)
        {
            appDbContext.Book.Update(book);
            await appDbContext.SaveChangesAsync();
            return book;
        }
    }
}
