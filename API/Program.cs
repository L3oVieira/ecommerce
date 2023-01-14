using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// config var for accessing the method that calls the json config file
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Connection Configs for the DB.
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddDbContext<StoreContext>(option => option.UseSqlite(config.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Apply Migrations
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

    try
    {
        var contextDb = scope.ServiceProvider.GetRequiredService<StoreContext>();
        await contextDb.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(contextDb, loggerFactory!);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory?.CreateLogger<Program>();
        logger?.LogError(ex, "An error occured during migration");
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
