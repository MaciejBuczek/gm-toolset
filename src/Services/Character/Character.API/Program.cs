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

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(options => { });

app.MapGet("/appinfo", () =>
{
    var appInfo = new
    {
        ServiceName = Assembly.GetExecutingAssembly().GetName().Name,
        Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
    };

    return appInfo;
});

app.MapCarter();

app.Run();