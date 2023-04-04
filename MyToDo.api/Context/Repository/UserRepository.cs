using MyToDo.api.Context.UnitOfWork;

namespace MyToDo.api.Context.Repository
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(MyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
