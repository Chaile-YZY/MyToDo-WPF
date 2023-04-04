using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class SummaryDto:BaseDto
    {
		private int sum;

		private int completedCount;

		private int memoCount;

		private string completedRadio=string.Empty;
		/// <summary>
		/// 待办事项列表
		/// </summary>

		private ObservableCollection<ToDoDto> todolist;
		/// <summary>
		/// 备忘录列表
		/// </summary>
		private ObservableCollection<MemoDto> memolist;


		public ObservableCollection<MemoDto> MemoList
		{
			get { return memolist; }
			set { memolist = value;OnPropertyChanged(); }
		}


		public ObservableCollection<ToDoDto> ToDoList
		{
			get { return todolist; }
			set { todolist = value; OnPropertyChanged(); }
		}


		public string CompletedRadio
		{
			get { return completedRadio; }
			set { completedRadio= value;OnPropertyChanged(); }
		}


		public int MemoCount
		{
			get { return memoCount; }
			set { memoCount = value;OnPropertyChanged(); }
		}


		public int CompletedCount
		{
			get { return completedCount; }
			set { completedCount = value;OnPropertyChanged(); }
		}


		public int Sum
		{
			get { return sum; }
			set { sum = value; OnPropertyChanged(); }
		}

	}
}
