using MyToDo.Common;
using MyToDo.Extension;
using MyToDo.Services;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        public LoginViewModel(ILoginService service,IEventAggregator aggregator)
        {
            UserDto=new RegisterUserDto();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.service = service;
            this.aggregator = aggregator;
        }

        private void Execute(string arg)
        {
            switch (arg) 
            {
                case "Login":Login();break;
                case "LoginOut":LoginOut();break;
                    //跳转注册页面
                case "Go":SelectedIndex = 1; break;
                    //返回登录页面
                case "Return": SelectedIndex = 0; break;
                    //注册账号
                case "Register": Register(); break;
            }
        }

        private  async void Register()
        {
            if (string.IsNullOrWhiteSpace(UserDto.Account)
                || string.IsNullOrWhiteSpace(UserDto.UserName)
                || string.IsNullOrWhiteSpace(UserDto.PassWord)
                || string.IsNullOrWhiteSpace(UserDto.NewPassWord)) 
            {
                aggregator.SendMessage("请输入完整的的注册信息！","Login");
                return;
            }
            if (UserDto.PassWord != UserDto.NewPassWord) 
            {
                //可以增加提示
                aggregator.SendMessage("两次的密码输入不一致,请检查","Login");
                return;
            }
          var RegisterResult =await service.RegisterAsync(new UserDto()
            { 
                    Account= UserDto.Account,
                    UserName= UserDto.UserName,
                    Password= UserDto.PassWord,
            });
            if (RegisterResult != null&& RegisterResult.Status) 
            {
                //注册成功
                aggregator.SendMessage("注册成功","Login");
                selectedIndex = 0;
                return;
            }
            aggregator.SendMessage(RegisterResult.Message, "Login");

        }

        void LoginOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        async void Login()
        {
            if (string.IsNullOrWhiteSpace(Account) ||
                string.IsNullOrWhiteSpace(PassWord)) return;
            var LoginResult =await service.LoginAsync(new Shared.Dtos.UserDto()
            {
                Account = Account,
                Password = PassWord
            });
            //登陆成功
            if (LoginResult!=null&&LoginResult.Status) 
            {
                AppSession.UserName =LoginResult.Result.UserName;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                return;
            }
            //登录失败提示
            aggregator.SendMessage(LoginResult.Message,"Login");
        }
        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value;RaisePropertyChanged(); }
        }


        public string Title { get; set; } = "ToDo";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            LoginOut();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        private string account;

        public string Account
        {
            get { return account; }
            set { account = value;RaisePropertyChanged(); }
        }

        private string password;
        private readonly ILoginService service;
        private readonly IEventAggregator aggregator;

        public string PassWord
        {
            get { return password; }
            set { password = value;RaisePropertyChanged(); }
        }

        private RegisterUserDto userDto;

        public RegisterUserDto UserDto
        {
            get { return userDto; }
            set { userDto = value; RaisePropertyChanged(); }
        }

    }
}
