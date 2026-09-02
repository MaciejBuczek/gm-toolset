var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.SetUpDI(builder.Configuration);

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