var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(builder.Configuration.GetConnectionString(Constants.ConnectionStringName) ?? string.Empty)));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var dbSetupHelper = app.Services.GetRequiredService<DbSetupHelper>();
    await dbSetupHelper.EnsureMigrations(app);
    await dbSetupHelper.EnsureRolesAreCreated(app);
}

app.UseHttpsRedirection();

app.Run();
