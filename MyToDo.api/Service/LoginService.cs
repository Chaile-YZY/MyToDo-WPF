using AutoMapper;
using MyToDo.api.Context;
using MyToDo.api.Context.UnitOfWork;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Extensions;

namespace MyToDo.api.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }   
        public async Task<ApiResponse> LoginAsync(string Account, string Password)
        {
            try
            {
                Password=Password.GetMD5();
                var model=await unitOfWork.GetRepository<User>() .GetFirstOrDefaultAsync(predicate:
                    x=>(x.Account.Equals(Account))&&
                    (x.Password.Equals(Password)));
                if(model == null ) 
                    return new ApiResponse("账号或密码错误，请重试！");
                return new ApiResponse(true,model);
            } 
            catch (Exception)
            {
                return new ApiResponse(false, "登录失败！");
            }
        }

        public async Task<ApiResponse> Register(UserDto user)
        {
            try
            {
                var model = mapper.Map<User>(user);
                var repository = unitOfWork.GetRepository<User>();
                var UserModel = await repository.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(model.Account));
                if (UserModel != null)
                    return new ApiResponse($"当前账号:{model.Account}已存在,请重新注册！");
                model.CreateDate = DateTime.Now;
                model.Password = model.Password.GetMD5();
                await repository.InsertAsync(model);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);
                return new ApiResponse(false, "注册失败，请稍后重试！");
            }
            catch (Exception )
            {
                return new ApiResponse(false, "注册账号失败！");
            }
        }
    }
}
