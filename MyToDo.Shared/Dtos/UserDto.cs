using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class UserDto : BaseDto
    {
        private string? userName;
        private string? account;
        private string? password;

        public string? UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        public string? Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged(); }
        }

        public string? Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

    }
}
