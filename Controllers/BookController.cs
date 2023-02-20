using Book.WebAPI.IServices;
using Book.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public  IBookService BookService { get; set; }
        public BookController(IBookService _BookService)
        {
            BookService = _BookService;
        }
        [HttpGet("GetAllBooks")]
        public IActionResult GetAll()
        {
            return Ok(BookService.GetAll());
        }

        [HttpPost("AddBook")]
        public IActionResult Post([FromBody] BookVM book)
        {
            BookService.PostBook(book);
            return Ok(BookService.Get(book?.Id));
        }
        [HttpPost("BulkInsert")]
        public IActionResult BulkInsert([FromBody] IEnumerable<BookVM> books)
        {
            BookService.BulkInsert(books);
            return Ok();
        }
        [HttpGet("GetBook/{id}")]
        public IActionResult Get(int id)
        {
            var res=BookService.Get(id);
            return Ok(res);
        }
    }
}
