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
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<OnboardingService>();
        services.AddScoped<SummaryService>();

        services.AddScoped<StartHandler>();
        services.AddScoped<TaskHandler>();
        services.AddScoped<ExpenseHandler>();
        services.AddScoped<ProfileHandler>();
        services.AddScoped<LanguageHandler>();
        services.AddScoped<HelpHandler>();
        services.AddScoped<SummaryHandler>();


        services.AddSingleton<ITelegramBotClient>(provider =>
        {
            var cfg = provider.GetRequiredService<IConfiguration>();
            var token = cfg["BotToken"]!;
            return new TelegramBotClient(token);
        });

        var serviceProvider = services.BuildServiceProvider();

        var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
        var start = serviceProvider.GetRequiredService<StartHandler>();
        var taskH = serviceProvider.GetRequiredService<TaskHandler>();
        var expenseH = serviceProvider.GetRequiredService<ExpenseHandler>();
        var profileH = serviceProvider.GetRequiredService<ProfileHandler>();
        var langH = serviceProvider.GetRequiredService<LanguageHandler>();
        var helpH = serviceProvider.GetRequiredService<HelpHandler>();
        var summaryH = serviceProvider.GetRequiredService<SummaryHandler>();

        bot.StartReceiving(
            async (client, update, ct) =>
            {
                if (update.Message is null) return;

                var text = update.Message.Text ?? string.Empty;
                if (text.StartsWith("/start"))
                    await start.HandleAsync(client, update.Message);
                else if (text.StartsWith("/task") || text.StartsWith("/tasks") || text.StartsWith("/done") || text.StartsWith("/delete"))
                    await taskH.HandleAsync(client, update.Message);
                else if (text.StartsWith("/expense") || text.StartsWith("/expenses"))
                    await expenseH.HandleAsync(client, update.Message);
                else if (text.StartsWith("/profile"))
                    await profileH.HandleAsync(client, update.Message);
                else if (text.StartsWith("/language") || update.Message.ReplyToMessage?.Text == "Choose language")
                    await langH.HandleAsync(client, update.Message);
                else if (text.StartsWith("/summary"))
                    await summaryH.HandleAsync(client, update.Message);
                else if (text.StartsWith("/help"))
                    await helpH.HandleAsync(client, update.Message);
                else
                {
                    var onboarding = serviceProvider.GetRequiredService<OnboardingService>();
                    var userService = serviceProvider.GetRequiredService<IUserService>();
                    var user = await userService.GetOrCreateAsync(update.Message.From!.Id);
                    await onboarding.HandleStepAsync(client, update.Message, user);
                }
            },
            (_, _, _) => Task.CompletedTask);

        Console.WriteLine("🤖 Dobby çalışıyor...");
        Console.ReadLine();
    }
}
