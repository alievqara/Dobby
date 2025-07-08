using Both.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Both.Handlers;

public class ExpenseHandler
{
    private readonly IExpenseService _expenseService;
    private readonly IUserService _userService;

    public ExpenseHandler(IExpenseService expenseService, IUserService userService)
    {
        _expenseService = expenseService;
        _userService = userService;
    }

    public async Task HandleAsync(ITelegramBotClient bot, Message message)
    {
        var text = message.Text?.Trim() ?? string.Empty;
        var parts = text.Split(' ', 3);
        var command = parts[0];
        var user = await _userService.GetOrCreateAsync(message.From!.Id);

        if (command == "/expense" && parts.Length >= 3 && decimal.TryParse(parts[1], out decimal amt))
        {
            var category = parts[2];
            await _expenseService.AddExpenseAsync(user.TelegramUserId, amt, category);
            await bot.SendTextMessageAsync(message.Chat.Id, "Expense added");
        }
        else if (command == "/expenses")
        {
            var expenses = await _expenseService.GetExpensesAsync(user.TelegramUserId);
            var lines = expenses.Select(e => $"{e.Id}. {e.Amount} {e.Category}");
            await bot.SendTextMessageAsync(message.Chat.Id, string.Join('\n', lines));
        }
    }
}
