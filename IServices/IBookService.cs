using Book.WebAPI.Models;

namespace Book.WebAPI.IServices
{
    public interface IBookService
    {
        public IEnumerable<BookVM>? GetAll();
        public BookVM? Get(int? id);
        public BookVM PostBook(BookVM book);
        public BookVM UpdateBook(BookVM book);
        public void BulkInsert(IEnumerable<BookVM> books);
    }
}
