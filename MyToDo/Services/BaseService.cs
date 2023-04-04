using MyToDo.Shared;
using MyToDo.Shared.Parameters;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryParameter = MyToDo.Shared.Parameters.QueryParameter;

namespace MyToDo.Services
{
    public class BaseService<TEntity>:IBaseService<TEntity>where TEntity : class
    {
        private readonly HttpRestClient restClient;
        private readonly string serviceName;

        public BaseService(HttpRestClient restClient,string serviceName) {
            this.restClient = restClient;
            this.serviceName = serviceName;
        }

        public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Add";
            request.Parameter = entity;
           return await restClient.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> DeleteAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Delete;
            request.Route = $"api/{serviceName}/Delete?id={id}";
            return await restClient.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter query)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/GetAll?pageIndex={query.PageIndex}" +
                $"&pageSize={query.PageSize}" +
                $"&search={query.Search}";
            request.Parameter = query;
            return await restClient.ExecuteAsync<PagedList<TEntity>>(request);
        }

        public async Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/Get?id={id}";
            return await restClient.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Update";
            request.Parameter = entity;
            return await restClient.ExecuteAsync<TEntity>(request);
        }
    }
}
