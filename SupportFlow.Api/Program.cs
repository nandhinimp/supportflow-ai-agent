var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
// builder.Services.AddSingleton<Microsoft.SemanticKernel.Kernel>();


// 🔹 Swagger services (IMPORTANT)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Enable Swagger ALWAYS (for now)
app.UseSwagger();
app.UseSwaggerUI();

// Middleware
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
