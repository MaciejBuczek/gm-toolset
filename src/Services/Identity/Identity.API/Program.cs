var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.SetupIdentity(builder.Configuration.GetConnectionString(Constants.ConnectionStringName) ?? string.Empty);
builder.Services.SetupDI();
builder.Services.SetupOptions(builder.Configuration);
builder.Services.AddHandlers();
builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddMessaging(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(options => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var dbSetupHelper = app.Services.GetRequiredService<DbSetupHelper>();
    await dbSetupHelper.EnsureMigrations(app);
    await dbSetupHelper.EnsureRolesAreCreated(app);
}

app.MapCarter();

app.Run();
