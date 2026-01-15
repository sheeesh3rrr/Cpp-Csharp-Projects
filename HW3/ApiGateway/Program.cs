using ApiGateway.Options;
using ApiGateway.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DownstreamOptions>(builder.Configuration.GetSection("Downstream"));
DownstreamOptions? downstream = builder.Configuration.GetSection("Downstream").Get<DownstreamOptions>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

int timeoutSeconds = downstream?.HttpTimeoutSeconds ?? 30;

builder.Services.AddHttpClient("filestoring", client =>
{
    client.BaseAddress = new Uri(downstream?.FileStoring?.BaseUrl ?? throw new InvalidOperationException("FileStoring BaseUrl not configured"));
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
});

builder.Services.AddHttpClient("fileanalysis", client =>
{
    client.BaseAddress = new Uri(downstream?.FileAnalysis?.BaseUrl ?? throw new InvalidOperationException("FileAnalysis BaseUrl not configured"));
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
});

builder.Services.AddScoped<IGatewayService, GatewayService>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
