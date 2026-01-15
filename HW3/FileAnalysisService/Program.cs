using FileAnalysisService.Data;
using FileAnalysisService.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FileAnalysisDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IFileAnalysisService, FileAnalysisManager>();
builder.Services.AddHttpClient();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    FileAnalysisDbContext db = scope.ServiceProvider.GetRequiredService<FileAnalysisDbContext>();
    _ = db.Database.EnsureCreated();
}

app.Run();
