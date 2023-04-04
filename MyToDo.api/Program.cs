using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyToDo.api.Context;
using MyToDo.api.Context.Repository;
using MyToDo.api.Context.UnitOfWork;
using MyToDo.api.Extensions;
using MyToDo.api.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SqliteDbConnection");
    options.UseSqlite(connectionString);
}).AddUnitOfWork<MyDbContext>()
.AddCustomRepository<ToDo, ToDoRepository>()
.AddCustomRepository<Memo, MemoRepository>()
.AddCustomRepository<User, UserRepository>();

builder.Services.AddTransient<IToDoService, ToDoService>();
builder.Services.AddTransient<IMemoService, MemoService>();
builder.Services.AddTransient<ILoginService, LoginService>();

//´´½¨Automapper
builder.Services.AddSingleton(new MapperConfiguration(config =>
{
    config.AddProfile(new AutoMapperProFile());

}).CreateMapper());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
