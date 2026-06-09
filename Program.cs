using Microsoft.EntityFrameworkCore;
using SmartStepsServer.Data;
using SmartStepsServer.Options;
using SmartStepsServer.Services;

var builder = WebApplication.CreateBuilder(args);

var AllowReactApp = "_allowReactApp";

// Add services to the container.
builder.Services.AddDbContext<SmartStepsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<SupabaseStorageOptions>(
    builder.Configuration.GetSection(SupabaseStorageOptions.SectionName));
builder.Services.Configure<PayOsOptions>(
    builder.Configuration.GetSection(PayOsOptions.SectionName));

builder.Services.AddHttpClient<ISupabaseAuthService, SupabaseAuthService>();
builder.Services.AddHttpClient<ISupabaseStorageService, SupabaseStorageService>();
builder.Services.AddHttpClient<IPayOsService, PayOsService>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowReactApp, policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS phải đặt trước Authorization
app.UseCors(AllowReactApp);

app.UseAuthorization();

app.MapControllers();

app.Run();
