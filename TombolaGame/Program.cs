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
                    cfg.Host("localhost", "/", h =>
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
