var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.SetupIdentity(builder.Configuration.GetConnectionString(Constants.ConnectionStringName) ?? string.Empty);
builder.Services.SetupDI();
builder.Services.AddHandlers();
builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler(options => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var dbSetupHelper = app.Services.GetRequiredService<DbSetupHelper>();
    await dbSetupHelper.EnsureMigrations(app);
    await dbSetupHelper.EnsureRolesAreCreated(app);
}

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
