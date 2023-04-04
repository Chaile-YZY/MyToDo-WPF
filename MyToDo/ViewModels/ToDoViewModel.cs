using MyToDo.Common;
using MyToDo.Extension;
using MyToDo.Services;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        public ToDoViewModel(IToDoService service, IContainerProvider provider) : base(provider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);
            dialogHost = provider.Resolve<IDialogHostService>();
            this.service = service;
            //CreateTodolist();
        }

        private async void Delete(ToDoDto obj)
        {
            var dialogResult = await dialogHost.Question("温馨提示",$"确认删除待办事项:{obj.Title}?");
            if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
            try
            { 
                UpdateLoading(true);
                var deleteResult = await service.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = ToDoDtos.FirstOrDefault(x => x.Id.Equals(obj.Id));
                    if (model != null)
                        ToDoDtos.Remove(model);
                }
            }
            finally 
            {
                UpdateLoading(false);
            }
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增":Add();break;
                case "查询":GetDataAsync();break;
                case "保存":Save();break;
            }
        }

        /// <summary>
        /// 下拉列表选中状态值
        /// </summary>
        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value;RaisePropertyChanged(); }
        }


        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentDto.Title) || 
                string.IsNullOrWhiteSpace(CurrentDto.Content))
                return;
            UpdateLoading(true);
            try
            {
                if (CurrentDto.Id > 0)
                {
                    var UpdateResult = await service.UpdateAsync(CurrentDto);
                    if (UpdateResult.Status)
                    {
                        var todo = ToDoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                            todo.Status = CurrentDto.Status;
                            IsRightDrawerOpen = false;
                        }
                    }
                }
                else
                {
                    var AddResult = await service.AddAsync(CurrentDto);
                    if (AddResult.Status)
                    {
                        ToDoDtos.Add(AddResult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
            finally 
            { 
                UpdateLoading(false);
            }
        }

        private string search;
        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return search; }
            set { search = value;RaisePropertyChanged(); }
        }


        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value;RaisePropertyChanged(); }
        }

        private ToDoDto currentDto; 
        /// <summary>
        /// 编辑选中/新增的对象
        /// </summary>
        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value;RaisePropertyChanged(); }
        }

        private void Add()
        {
            CurrentDto = new ToDoDto();
            IsRightDrawerOpen = true;   
        }
        private async void Selected(ToDoDto obj)
        {
            UpdateLoading(true);
            try
            {
                var todoResult = await service.GetFirstOfDefaultAsync(obj.Id);
                if (todoResult.Status)
                {
                    CurrentDto = todoResult.Result;
                    IsRightDrawerOpen = true;
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> SelectedCommand{ get; private set; }
        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }

        private ObservableCollection<ToDoDto> toDoDtos;
        private readonly IToDoService service;

        public ObservableCollection<ToDoDto>ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value;}
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        async void GetDataAsync()
        {
            UpdateLoading(true);
            try
            {
                int? status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;
                var TodoResult = await service.GetAllFilterAsync(new ToDoParameter()
                {
                    PageIndex = 0,
                    PageSize = 100,
                    Search = Search,
                    Status = status
                });
                if (TodoResult.Status)
                {
                    ToDoDtos.Clear();
                    foreach (var item in TodoResult.Result.Items)
                    {
                        ToDoDtos.Add(item);
                    }
                }
            }
            finally 
            {
                UpdateLoading(false);
            }
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (navigationContext.Parameters.ContainsKey("Value"))
                SelectedIndex = navigationContext.Parameters.GetValue<int>("Value");
            else
                SelectedIndex = 0;
            GetDataAsync();
        }
    }
}
