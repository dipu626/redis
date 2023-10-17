using Caching.Application.Features.Queries;
using Caching.Infrastructure.Persistence;
using Caching.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Scrutor;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
{
    builder.Services.AddDbContext<CacheDbContext>(options =>
    {
        options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration: builder.Configuration.GetConnectionString("RedisUrl")));
}

// Add MediatR
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetProductsQuery).Assembly));
}

// Add Repositories & Services
{
    //builder.Services.AddScoped<ICacheRepository, CacheRepository>();
    //builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.Scan(service => service.FromAssemblies(typeof(CacheRepository).Assembly)
                                            .AddClasses()
                                            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                                            .AsMatchingInterface()
                                            .WithScopedLifetime());
}

// Add Validators
{

}

var app = builder.Build();

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
