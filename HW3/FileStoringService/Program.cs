using FileStoringService.Data;
using FileStoringService.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FileStoringDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IFileStorageService, FileStorageManager>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    FileStoringDbContext db = scope.ServiceProvider.GetRequiredService<FileStoringDbContext>();
    _ = db.Database.EnsureCreated();
}

app.Run();
