using Both.Data;
using Both.Handlers;
using Both.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

class Program
{
    static async Task Main()
    {
        // Config yükle
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // DI container
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


        services.AddScoped<IUserService, UserService>();
        services.AddScoped<OnboardingService>();
        services.AddScoped<StartHandler>();


        services.AddSingleton<ITelegramBotClient>(provider =>
            new TelegramBotClient("7689085384:AAFjb6WeoKZbptkQ6uEqDzsofJdo5GNaxxc"));

        var serviceProvider = services.BuildServiceProvider();

        var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
        var userService = serviceProvider.GetRequiredService<IUserService>();

        // Event bağlama
        bot.StartReceiving(
            async (client, update, ct) =>
            {
                if (update.Message is not null)
                {
                    var user = await userService.GetOrCreateAsync(update.Message.From!.Id);
                    var onboarding = serviceProvider.GetRequiredService<OnboardingService>();
                    await onboarding.HandleStepAsync(client, update.Message, user);
                }
            },
            (_, _, _) => Task.CompletedTask);

        Console.WriteLine("🤖 Dobby çalışıyor...");
        Console.ReadLine();
    }
}
