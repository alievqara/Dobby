using Telegram.Bot;
using Telegram.Bot.Types;

namespace Both.Handlers;

public class HelpHandler
{
    public async Task HandleAsync(ITelegramBotClient bot, Message message)
    {
        string help = "/task <desc> - add task\n/tasks - list\n/done <id> - mark done\n/delete <id> - delete\n/expense <amount> <cat> - add expense\n/expenses - list\n/profile - show profile\n/summary - summary";
        await bot.SendTextMessageAsync(message.Chat.Id, help);
    }
}
