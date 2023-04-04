using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Extension;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyToDo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IDialogHostService dialogHost;

        public MainView(IEventAggregator eventAggregator,IDialogHostService dialogHost)
        {
            InitializeComponent();

            eventAggregator.RegisterMessage(arg =>
            {
                Snackbar.MessageQueue.Enqueue(arg.Message);
            },"Main");

            //注册等待消息窗口
            eventAggregator.Register(arg =>
            {
                DialogHost.IsOpen = arg.IsOpen;
                if(DialogHost.IsOpen )
                    DialogHost.DialogContent = new ProgressView();
            });
            menuBar.SelectionChanged += (s, e) =>
            {
               drawerHost.IsLeftDrawerOpen = false;
            };
            BtnMin.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };
            BtnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            };
            BtnClose.Click += async (s, e) =>
            {
               var dialogResult =await dialogHost.Question("温馨提示", "确认退出系统?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                this.Close();
            };
            ColorZone.MouseMove += (s, e) => {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                };
            };
            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                };
            };
            this.dialogHost = dialogHost;
        }

        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
