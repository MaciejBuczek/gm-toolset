using Common.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMartenConnection(builder.Configuration);
builder.Services.AddCharacterRepository();
builder.Services.AddHandlers();
builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(options => { });

app.MapCarter();

app.Run();