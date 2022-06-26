using Discount.Grpc.Repositories;
using Discount.Grpc.Entities;
using Discount.Grpc.Extensions;
using Discount.Grpc.Services;
//using Discount.Grpcs.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseEndpoints(points =>
{
    points.MapGrpcService<DiscountService>();

    //points.MapGet("/", async context =>
    //{
    //    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    //});
});

app.MigrateDatabase<Coupon>(10);

app.Run();