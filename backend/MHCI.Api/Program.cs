using MHCI.Api.Interceptors;
using MHCI.Application.Interfaces;
using MHCI.Application.Services;
using MHCI.Domain.Repositories;
using MHCI.Infrastructure.Persistence;
using MHCI.Infrastructure.Persistence.CheckIns;
using MHCI.Infrastructure.Seedings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnStr")));


builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();
builder.Services.AddScoped<ICheckInRepository, CheckInRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

app.UseCors("AllowFrontend");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbInitializer.Initialize(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseMiddleware<HeaderInterceptor>();

app.UseAuthorization();

app.MapControllers();

app.Run();
