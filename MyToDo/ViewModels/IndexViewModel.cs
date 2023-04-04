using MyToDo.Common;
using MyToDo.Common.Events;
using MyToDo.Common.Models;
using MyToDo.Extension;
using MyToDo.Services;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        private readonly IDialogHostService dialog;
        private ObservableCollection<TaskBar> taskBars;
        private readonly IRegionManager regionManager;
        private SummaryDto summary;

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value;RaisePropertyChanged(); }
        }


        public IndexViewModel(IDialogHostService dialog,IContainerProvider container):base(container)
        {
            Title = $"你好,{AppSession.UserName}  {DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";
            CreateTaskBars();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.toDoService=container.Resolve<IToDoService>();
            this.memoService = container.Resolve<IMemoService>();
            this.regionManager = container.Resolve<IRegionManager>();
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            ToDoCompltedCommand = new DelegateCommand<ToDoDto>(Completed);
            NavigateCommand = new DelegateCommand<TaskBar>(Navigate);
            this.dialog = dialog;
        }

        private void Navigate(TaskBar obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Target)) return;
            NavigationParameters param=new NavigationParameters();
            if (obj.Title == "已完成") 
            {
                param.Add("Value", 2);
            }
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target, param);
        }

        private async void Completed(ToDoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var updateResult = await toDoService.UpdateAsync(obj);
                if (updateResult.Status)
                {
                    var toDo = summary.ToDoList.FirstOrDefault(x => x.Id.Equals(obj.Id));
                    if (toDo != null)
                    {
                        Summary.ToDoList.Remove(toDo);
                        Summary.CompletedCount += 1;
                        Summary.CompletedRadio = (Summary.CompletedCount / (double)Summary.Sum).ToString("0%");
                        this.Refresh();
                    }
                    aggregator.SendMessage("已完成", "Main");
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        #region 属性
        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> ToDoCompltedCommand { get; private set; }
        public DelegateCommand<TaskBar> NavigateCommand { get; private set; }

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

       

        /// <summary>
        /// 首页统计
        /// </summary>
        public SummaryDto Summary
        {
            get { return summary; }
            set { summary = value;RaisePropertyChanged(); }
        }


        #endregion

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo(null); break;
                case "新增备忘录": AddMemo(null); break;
            }
        }
        /// <summary>
        /// 添加待办事项
        /// </summary>
        async void AddToDo(ToDoDto model) 
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
                param.Add("Value",model);
           var dialogResult=await dialog.ShowDialog("AddToDoView",param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var toDo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                    if (toDo.Id > 0)
                    {
                        var UpdateResult = await toDoService.UpdateAsync(toDo);
                        if (UpdateResult.Status)
                        {
                            var todomodel = Summary.ToDoList.FirstOrDefault(x => x.Id.Equals(toDo.Id));
                            if (todomodel != null)
                            {
                                todomodel.Title = toDo.Title;
                                todomodel.Content = toDo.Content;
                            }
                        }
                    }
                    else
                    {
                        var addResult = await toDoService.AddAsync(toDo);
                        if (addResult.Status)
                        {
                            Summary.ToDoList.Add(addResult.Result);
                            Summary.Sum += 1;
                            Summary.CompletedRadio = (Summary.CompletedCount / (double)Summary.Sum).ToString("0%");
                            this.Refresh();
                        }
                        aggregator.SendMessage("已成功添加", "Main");
                    }
                }
                finally 
                {
                    UpdateLoading(false);
                }
            }
        }
        /// <summary>
        /// 添加备忘录
        /// </summary>
        async void AddMemo(MemoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
                param.Add("Value", model);
            var dialogResult=await dialog.ShowDialog("AddMemoView", param);
            if(dialogResult.Result== ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");
                    if (memo.Id > 0)
                    {
                        var UpdateResult = await memoService.UpdateAsync(memo);
                        if (UpdateResult.Status)
                        {
                            var memoModel = Summary.MemoList.FirstOrDefault(x => x.Id.Equals(memo.Id));
                            if (memoModel != null)
                            {
                                memoModel.Title = memo.Title;
                                memoModel.Content = memo.Content;
                            }
                        }
                    }
                    else
                    {
                        var addResult = await memoService.AddAsync(memo);
                        if (addResult.Status)
                        {
                            Summary.MemoList.Add(addResult.Result);
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
            }
        }

        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar()
            {
                Icon = "ClockFast",
                Title = "汇总",
                Color = "#0097FF",
                Target = "ToDoView"
            });
            TaskBars.Add(new TaskBar()
            {
                Icon = "ClockCheckOutline",
                Title = "已完成",
                Color = "#28B84B",
                Target = "ToDoView"
            });
            TaskBars.Add(new TaskBar()
            {
                Icon = "ChartLineVariant",
                Title = "完成比例",
                Color = "#00B4DF",
                Target = ""
            });
            TaskBars.Add(new TaskBar()
            {
                Icon = "PlaylistStar",
                Title = "备忘录",
                Color = "#FFA200",
                Target = "MemoView"
            });
        }
        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
           var summaryResult =await toDoService.SummaryAsync();
            if (summaryResult.Status)
            {
                Summary = summaryResult.Result;//接受统计结果
                Refresh();
            }
            base.OnNavigatedTo(navigationContext);
        }
        void Refresh ()
        {
            TaskBars[0].Content=summary.Sum.ToString();
            TaskBars[1].Content = summary.CompletedCount.ToString();
            TaskBars[2].Content = summary.CompletedRadio;
            TaskBars[3].Content = summary.MemoCount.ToString();
        }
    }
}
