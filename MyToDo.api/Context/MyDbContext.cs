using Microsoft.EntityFrameworkCore;

namespace MyToDo.api.Context
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<ToDo>? ToDos { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Memo>? Memos { get; set; }
    }
}
