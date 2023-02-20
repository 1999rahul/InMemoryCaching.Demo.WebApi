using Book.WebAPI.IServices;
using Book.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Book.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Deafult Services.

            builder.Services.AddControllers();
           
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var Services = builder.Services;

            

            //Added Services
            Services.AddDbContext<BookDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:sqlConnection").Value);
                
            });
            Services.AddMemoryCache();

            //Custom Services
            Services.AddTransient<IBookService, BookService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}