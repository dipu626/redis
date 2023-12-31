using Caching.Application.Features.Queries;
using Caching.Application.Features.Queries.Validators;
using Caching.Application.Mapping;
using Caching.Infrastructure.Persistence;
using Caching.Infrastructure.Providers;
using FluentValidation;
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
    builder.Services.Scan(srv => srv.FromAssemblies(typeof(CacheRepository).Assembly)
                                    .AddClasses()
                                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                                    .AsMatchingInterface()
                                    .WithScopedLifetime());
}

// Add Validators
{
    builder.Services.AddValidatorsFromAssembly(typeof(GetProductQueryValidator).Assembly);
}

// Add Automapper
{
    builder.Services.AddAutoMapper(typeof(CachingProfile).Assembly);
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
