namespace MyToDo.api.Context
{
    public class User:BaseEntity
    {
        public string Account { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
