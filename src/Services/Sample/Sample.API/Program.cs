using Carter;
using Common.Exceptions.Handler;
using FluentValidation;
using Sample.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHandlers();

builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Add services to the container.

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseExceptionHandler(options => { });


app.MapGet("/HelloAzure", () =>
{
    return "Hello I live in the Azure Container App now!";
});

app.MapCarter();

app.Run();
