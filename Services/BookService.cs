using Book.WebAPI.IServices;
using Book.WebAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Book.WebAPI.Services
{
    public class BookService : IBookService
    {
        BookDBContext _context;
        private const string bookListCacheKey = "BookList";
        private IMemoryCache _cache;
        private ILogger<BookService> _logger;
        public BookService(BookDBContext context, IMemoryCache cache, ILogger<BookService> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public void BulkInsert(IEnumerable<BookVM> books)
        {
            foreach(var book in books)
            {
                _context.Books.Add(book);
            }
            _context.SaveChanges();
        }

        public BookVM? Get(int? id)
        {
            
                return _context.Books.Find(id);
            
        }

        public IEnumerable<BookVM>? GetAll()
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of books from cache.");

            List<BookVM>? books;
            books= _cache.Get<List<BookVM>>(bookListCacheKey);

            if (books == null)
            {
                _logger.Log(LogLevel.Information, "Bookdata missing from cache,Fetching book data from Database");
                books =_context.Books.ToList();
                _cache.Set(bookListCacheKey, books,TimeSpan.FromMinutes(value:1));
            }
            else
            {
                _logger.Log(LogLevel.Information, "Book data found in cache memory,Fetching data from cache.");
            }
            return books;   
        }

        public BookVM? PostBook(BookVM book)
        {
           
            _context.Add(book);
            _context.SaveChanges();
            return Get(book.Id);


        }

        public BookVM UpdateBook(BookVM book)
        {
           
           
                _context.Add(book);
                _context.SaveChanges();
                return book;
            
        }
    }
}
