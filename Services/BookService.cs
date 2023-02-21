using Book.WebAPI.IServices;
using Book.WebAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Book.WebAPI.Services
{
    public class BookService : IBookService
    {
        BookDBContext _context;
        private const string bookListCacheKey = "BookList";
        //private IMemoryCache _cache;
        private ILogger<BookService> _logger;
        private readonly IDistributedCache _cache;
        public BookService(BookDBContext context, IDistributedCache cache, ILogger<BookService> logger)
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

        public BookVM? Get(int? id)=> _context.Books.Find(id);
       
        public IEnumerable<BookVM>? GetAll()
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of books from cache.");

            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            List<BookVM> books=new();
            var cachedBooks=_cache.GetString(bookListCacheKey);
            if (string.IsNullOrEmpty(cachedBooks))
            {
                _logger.Log(LogLevel.Information, "Bookdata missing from cache,Fetching book data from Database");
                books =_context.Books.ToList();
                DistributedCacheEntryOptions options = new();
                options.SetAbsoluteExpiration(new TimeSpan(0, 0, 30));
                _cache.SetString(bookListCacheKey, JsonConvert.SerializeObject(books), options);
            }
            else
            {
                _logger.Log(LogLevel.Information, "Book data found in cache memory,Fetching data from cache.");
                books = JsonConvert.DeserializeObject<List<BookVM>>(cachedBooks);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            _logger.Log(LogLevel.Information, $"It took {elapsedMs} ms to fetch the data");
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
