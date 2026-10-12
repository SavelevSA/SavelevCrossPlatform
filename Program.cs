using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Auth;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<LabDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LabDbContext")
        ?? throw new InvalidOperationException("Не задана строка подключения LabDbContext.")));
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<LabWorkService>();
builder.Services.AddScoped<ClassroomService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<QueryService>();
var tokens = new DemoTokenService();
builder.Services.AddSingleton(tokens);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = tokens.ValidationParameters);
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LabDbContext>();
    await db.Database.MigrateAsync();
    await LabDbSeeder.SeedAsync(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
