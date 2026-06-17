using Microsoft.EntityFrameworkCore;

namespace CoursAPI
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Models.TodoItem> TodoItems { get; set; }


        public DbSet<Models.ProjetItem> ProjetItems { get; set; }
    }
}
