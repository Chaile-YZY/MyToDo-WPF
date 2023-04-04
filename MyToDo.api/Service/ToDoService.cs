using AutoMapper;
using MyToDo.api.Context;
using MyToDo.api.Context.UnitOfWork;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Collections.ObjectModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyToDo.api.Service
{
    public class ToDoService : IToDoService
    {
        /// <summary>
        /// 待办事项实现
        /// </summary>
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            try
            {
                var todo=mapper.Map<ToDo>(model);
                await unitOfWork.GetRepository<ToDo>().InsertAsync(todo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse(false, "添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository=unitOfWork.GetRepository<ToDo>();
                var todo =await repository
                     .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(todo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "");
                return new ApiResponse(false, "删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter query)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(predicate:
                    x=>string.IsNullOrWhiteSpace(query.Search) || x.Title.Contains(query.Search),
                    pageIndex: query.PageIndex,
                    pageSize:query.PageSize,
                    orderBy:source=>source.OrderByDescending(t=>t.CreateDate));
                return new ApiResponse(true,todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(ToDoParameter toDoParameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(predicate:
                x => (string.IsNullOrWhiteSpace(toDoParameter.Search)? true: x.Title.Contains(toDoParameter.Search))
                && (toDoParameter.Status ==null? true:x.Status.Equals(toDoParameter.Status)),
                    pageIndex: toDoParameter.PageIndex,
                    pageSize: toDoParameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var todo = await repository
                    .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, todo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> Summary()
        {
            try
            {
                //待办事项结果
                var todos = await unitOfWork.GetRepository<ToDo>().GetAllAsync(
                    orderBy:s=>s.OrderByDescending(t=>t.CreateDate));
                //备忘录结果
                var memos = await unitOfWork.GetRepository<Memo>().GetAllAsync(
                   orderBy: s => s.OrderByDescending(t => t.CreateDate));
                SummaryDto summary = new SummaryDto();
                //所有待办事项数量
                summary.Sum = todos.Count();
                //完成总数
                summary.CompletedCount= todos.Where(t=>t.Status==1).Count();
                //完成比例
                summary.CompletedRadio = (summary.CompletedCount /(double)summary.Sum).ToString("0%");
                
                //备忘录数量
                summary.MemoCount = memos.Count();
                    
                summary.ToDoList = new ObservableCollection<ToDoDto>(
                    mapper.Map<List<ToDoDto>>(todos.Where(t=>t.Status==0)));
                summary.MemoList = new ObservableCollection<MemoDto>(
                    mapper.Map<List<MemoDto>>(memos));

                return new ApiResponse(true,summary);

            }
            catch (Exception)
            {
                return new ApiResponse(false,"");
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            try
            {
                var Dbtodo = mapper.Map<ToDo>(model);
                var repository = unitOfWork.GetRepository<ToDo>();
                var todo = await repository
                    .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(Dbtodo.Id));
                todo.Title = Dbtodo.Title;
                todo.Content= Dbtodo.Content;
                todo.UpdateDate = DateTime.Now;
                todo.Status = Dbtodo.Status;
                repository.Update(todo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse(false, "更新数据异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
