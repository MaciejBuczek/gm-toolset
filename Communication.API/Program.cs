using Carter;
using Communication.API;
using Common.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.SetUpDI(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwtSwaggerGen();
builder.Services.AddCarter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(options => { });
app.MapCarter();
app.UseAuthentication();
app.UseAuthorization();
app.Run();