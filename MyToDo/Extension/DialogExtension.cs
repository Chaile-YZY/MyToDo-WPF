using MyToDo.Common;
using MyToDo.Common.Events;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Extension
{
    public static class DialogExtension
    {
        /// <summary>
        /// 询问窗口，指定dialoghost会话主机
        /// </summary>
        /// <param name="dialogHost"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="dialogHostName"></param>
        /// <returns></returns>
        public static async Task<IDialogResult> Question(this IDialogHostService dialogHost,
            string title, string content, string dialogHostName = "Root")
        { 
            DialogParameters param = new DialogParameters();
            param.Add("Title", title);
            param.Add("Content", content);
            param.Add("dialogHostName", dialogHostName);
            var dialogResult=await dialogHost.ShowDialog("MsgView", param, dialogHostName);
            return  dialogResult;

        }
        /// 推送等待消息
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <param name="updateModel"></param>
        public static void UpdateLoading(this IEventAggregator eventAggregator,UpdateModel updateModel) 
        {
            eventAggregator.GetEvent<UpdateLoadingEvent>().Publish(updateModel);
        }
        /// <summary>
        /// 注册等待消息
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <param name="action"></param>
        public static void Register(this IEventAggregator eventAggregator,Action<UpdateModel> action)
        {
            eventAggregator.GetEvent<UpdateLoadingEvent>().Subscribe(action);
        }

        /// <summary>
        /// 注册提示消息事件
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void RegisterMessage(this IEventAggregator aggregator,
            Action<MessageModel> action,string filterName="Main" )
        {
            aggregator.GetEvent<MessageEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m)=> 
                {
                    return m.Filter.Equals(filterName);
                });
        }
        /// <summary>
        /// 发布消息事件
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="message"></param>
        public static void SendMessage(this IEventAggregator aggregator,string message, string filterName = "Main")
        {
            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel() 
            { 
                Filter=filterName,
                Message=message
            });
        }

    }
}
