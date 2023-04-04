using MyToDo.Common.Models;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Services
{
    public class ToDoService : BaseService<ToDoDto>, IToDoService
    {
        private readonly HttpRestClient restClient;

        public ToDoService(HttpRestClient restClient) : base(restClient,"ToDo")
        {
            this.restClient = restClient;
        }

        public async Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter toDoParameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/ToDo/GetAll?pageIndex={toDoParameter.PageIndex}" +
            $"&pageSize={toDoParameter.PageSize}" +
            $"&search={toDoParameter.Search}" +
            $"&status={toDoParameter.Status}";
            request.Parameter = toDoParameter;
            return await restClient.ExecuteAsync<PagedList<ToDoDto>>(request);
        }

        public async Task<ApiResponse<SummaryDto>> SummaryAsync()
        {
            BaseRequest request= new BaseRequest();
            request.Route = "api/ToDo/Summary";
            return await restClient.ExecuteAsync<SummaryDto>(request);
        }
    }
}
