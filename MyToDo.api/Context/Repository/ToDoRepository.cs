using Microsoft.EntityFrameworkCore;
using MyToDo.api.Context.UnitOfWork;

namespace MyToDo.api.Context.Repository
{
    public class ToDoRepository : Repository<ToDo>, IRepository<ToDo>
    {
        public ToDoRepository(MyDbContext dbContext) : base(dbContext)
        {

        }
    }
}
