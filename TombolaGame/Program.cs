using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TombolaGame.Data;
using TombolaGame.Middlewares;
using TombolaGame.Repositories;
using TombolaGame.Repositories.Contracts;
using TombolaGame.Services;
using TombolaGame.WinnerSelection;
using TombolaGame.WinnerSelector.Strategies;

namespace TombolaGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    // Change tombola.rabbitmq to localhost for local testing
                    cfg.Host("tombola.rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });

            // Add services to the container.

            builder.Services.AddControllers()
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
             });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<TombolaContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Services
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IAwardService, AwardService>();
            builder.Services.AddScoped<ITombolaService, TombolaService>();


            //Repositories
            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<IAwardRepository, AwardRepository>();
            builder.Services.AddScoped<ITombolaRepository, TombolaRepository>();

            builder.Services.AddScoped<RandomSelectionStrategy>();
            builder.Services.AddScoped<WeightedSelectionStrategy>();
            builder.Services.AddScoped<OnePrizePerPlayerStrategy>();
            builder.Services.AddScoped<IWinnerSelectionStrategyFactory, WinnerSelectionStrategyFactory>();
            builder.Services.AddScoped<IWinnerSelectionService, WinnerSelectionService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TombolaContext>();
                var retries = 5;
                var delay = TimeSpan.FromSeconds(5);

                for (int i = 0; i < retries; i++)
                {
                    try
                    {
                        db.Database.Migrate();
                        break; 
                    }
                    catch (Exception ex)
                    {
                        if (i == retries - 1)
                            throw; 

                        Console.WriteLine($"Migration was unsuccessful, attempt {i + 1}/{retries}." +
                            $" Error: {ex.Message}. Will retry after {delay.TotalSeconds} seconds.");
                        Thread.Sleep(delay);
                    }
                }
            }

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

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
        }
    }
}
