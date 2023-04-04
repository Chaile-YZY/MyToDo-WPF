using Microsoft.EntityFrameworkCore;
using MyToDo.api.Context.UnitOfWork;

namespace MyToDo.api.Context.Repository
{
    public class MemoRepository : Repository<Memo>, IRepository<Memo>
    {
        public MemoRepository(MyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
