using Microsoft.AspNetCore.Mvc;
using MyToDo.api.Service;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using ApiResponse = MyToDo.api.Service.ApiResponse;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//备忘录
namespace MyToDo.api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemoController : ControllerBase
    {
        private readonly IMemoService memoService;

        public MemoController(IMemoService memoService)
        {
            this.memoService = memoService;
        }
        [HttpGet]
        public async Task<ApiResponse> Get(int id) => await memoService.GetSingleAsync(id);
        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery]QueryParameter query) => await memoService.GetAllAsync(query);
        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MemoDto model) => await memoService.AddAsync(model);
        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] MemoDto model) => await memoService.UpdateAsync(model);
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await memoService.DeleteAsync(id);
    }
}
