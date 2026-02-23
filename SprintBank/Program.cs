using Microsoft.EntityFrameworkCore;
using SprintBank.Data;
using SprintBank.Mapping;
using SprintBank.Servcies.Implementation;
using SprintBank.Servcies.Interface;
using SprintBank.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(option=> 
option.UseSqlServer(builder.Configuration.GetConnectionString("default")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddAutoMapper(typeof(AccountMapping),typeof(TransactionMapping));

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
