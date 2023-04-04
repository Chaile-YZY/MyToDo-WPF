using AutoMapper;
using MyToDo.api.Context.UnitOfWork;
using MyToDo.api.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.api.Service
{
    public class MemoService :IMemoService
    {
        /// <summary>
        /// 备忘录实现
        /// </summary>
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            try
            {
                var Memo = mapper.Map<Memo>(model);
                await unitOfWork.GetRepository<Memo>().InsertAsync(Memo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, Memo);
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
                var repository = unitOfWork.GetRepository<Memo>();
                var memo = await repository
                     .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(memo);
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
                var repository = unitOfWork.GetRepository<Memo>();
                var memos = await repository.GetPagedListAsync(predicate:
                    x => string.IsNullOrWhiteSpace(query.Search) || x.Title.Contains(query.Search),
                    pageIndex: query.PageIndex,
                    pageSize: query.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));    
                return new ApiResponse(true, memos);
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
                var repository = unitOfWork.GetRepository<Memo>();
                var memo = await repository
                    .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, memo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            try
            {
                var Dbmemo = mapper.Map<Memo>(model);
                var repository = unitOfWork.GetRepository<Memo>();
                var memo = await repository
                    .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(Dbmemo.Id));
                memo.Title = Dbmemo.Title;
                memo.Content = Dbmemo.Content;
                memo.UpdateDate = DateTime.Now;
                repository.Update(memo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, memo);
                return new ApiResponse(false, "更新数据异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
