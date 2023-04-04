using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    public class TaskBar:BindableBase
    {
		//任务栏
		private string icon;
        private string title;
        private string content;
        private string color;
        private string target;


        public string Icon
		{
			get { return icon; }
			set { icon = value; }
		}

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

		public string Content
		{
			get { return content; }
			set { content = value;RaisePropertyChanged(); }
		}

		public string Color
		{
			get { return color; }
			set { color = value; }
		}
		/* 触发目标*/
		public string Target
		{
			get { return target; }
			set { target = value; }
		}
	}
}
