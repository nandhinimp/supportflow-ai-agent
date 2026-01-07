#pragma warning disable SKEXP0010

using Microsoft.SemanticKernel;
using SupportFlow.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Semantic Kernel + Ollama (Local LLM)
builder.Services.AddSingleton(sp =>
{
    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddOpenAIChatCompletion(
        modelId: "qwen2.5:3b",
        endpoint: new Uri("http://localhost:11434/v1"),
        apiKey: "ollama" // dummy value
    );

    return kernelBuilder.Build();
});

// 🔹 Register TOOL
builder.Services.AddSingleton<OrderService>();

// 🔹 ASP.NET Core services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PolicyService>();
builder.Services.AddSingleton<EscalationService>();


var app = builder.Build();

// 🔹 Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
