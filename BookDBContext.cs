using Book.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Book.WebAPI
{
    public class BookDBContext:DbContext
    {
        public DbSet<BookVM> Books { get; set; }
        public BookDBContext(DbContextOptions<BookDBContext> options):base(options){ }
    }
}
