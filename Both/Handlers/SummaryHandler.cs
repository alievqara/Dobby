using Both.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Both.Handlers;

public class SummaryHandler
{
    private readonly SummaryService _summaryService;
    private readonly IUserService _userService;

    public SummaryHandler(SummaryService summaryService, IUserService userService)
    {
        _summaryService = summaryService;
        _userService = userService;
    }

    public async Task HandleAsync(ITelegramBotClient bot, Message message)
    {
        var user = await _userService.GetOrCreateAsync(message.From!.Id);
        var text = await _summaryService.GetSummaryAsync(user.TelegramUserId);
        await bot.SendTextMessageAsync(message.Chat.Id, text);
    }
}
