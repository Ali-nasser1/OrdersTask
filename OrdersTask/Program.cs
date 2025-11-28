
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrdersTask.APIs.Middlewares;
using OrdersTask.Application.Interfaces;
using OrdersTask.Application.Services;
using OrdersTask.Application.Validators;
using OrdersTask.Domain.Interfaces;
using OrdersTask.Infrastructure.Cache;
using OrdersTask.Infrastructure.Data;
using OrdersTask.Infrastructure.Repositories;
using Serilog;
using StackExchange.Redis;

namespace OrdersTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/ordersapi-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(OP =>
            {
                var configuration = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(configuration!);
            });

            // Repositories
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // Services
            builder.Services.AddScoped<ICacheService, RedisCacheService>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            // Validators
            builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();



            var app = builder.Build();


            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
