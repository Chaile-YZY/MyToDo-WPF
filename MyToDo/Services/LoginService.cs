using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName="Login";
        public LoginService(HttpRestClient client) 
        {
            this.client = client;
        }
        public async Task<ApiResponse<UserDto>> LoginAsync(UserDto dto)
        {
            BaseRequest baseRequest=new BaseRequest();
            baseRequest.Method = RestSharp.Method.Post;
            baseRequest.Route = $"api/{serviceName}/Login";
            baseRequest.Parameter = dto;
            return await client.ExecuteAsync<UserDto>(baseRequest);
        }

        public async Task<ApiResponse> RegisterAsync(UserDto dto)
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.Post;
            baseRequest.Route = $"api/{serviceName}/Register";
            baseRequest.Parameter = dto;
            return await client.ExecuteAsync(baseRequest);
        }
    }
}
