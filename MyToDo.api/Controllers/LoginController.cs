using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using MyToDo.api.Service;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using ApiResponse = MyToDo.api.Service.ApiResponse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyToDo.api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }
        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto param)
        {
            return await loginService.LoginAsync(Account: param.Account, password: param.Password);
        }

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto user) =>
            await loginService.Register(user);
    }
}
