// builder configuration
using DiscountCodesGenerator.Services.DiscountCodes.Generate;
using DiscountCodesGenerator.Services.DiscountCodes.Get;
using DiscountCodesGenerator.Services.DiscountCodes.Consume;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProjectServices(builder.Configuration);

MapsterConfig.RegisterMappings();

// app configurations
var app = builder.Build();

// Initialize migrations
app.ApplyMigrations();

// Initialize gRPC services
app.MapGrpcService<GeneratorService>();
app.MapGrpcService<DiscountCodesGenerator.Services.DiscountCodes.Consume.Endpoint>();
app.MapGrpcService<DiscountCodesGenerator.Services.DiscountCodes.Get.Endpoint>();
app.MapGet("/", () => "Service not available for HTTP requests.");

// Enable gRPC reflection for non-prod environments
if (!app.Environment.IsProduction())
    app.MapGrpcReflectionService();

app.Run();

