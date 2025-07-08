// builder configuration
using DiscountCodesGenerator.Services.DiscountCodes.GenerateCodeService;
using DiscountCodesGenerator.Services.DiscountCodes.UseDiscountCodeService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProjectServices(builder.Configuration);

// app configurations
var app = builder.Build();

// Initialize migrations
app.ApplyMigrations();

// Initialize gRPC services
app.MapGrpcService<GeneratorService>();
app.MapGrpcService<UseDiscountCodeService>();
app.MapGet("/", () => "Service not available for HTTP requests.");

// Enable gRPC reflection for non-prod environments
if (!app.Environment.IsProduction())
    app.MapGrpcReflectionService();

app.Run();

