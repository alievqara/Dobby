using Both.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Both.Handlers;

public class ProfileHandler
{
    private readonly IUserService _userService;

    public ProfileHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task HandleAsync(ITelegramBotClient bot, Message message)
    {
        var user = await _userService.GetOrCreateAsync(message.From!.Id);
        var info = $"Name: {user.Name}\nAge: {user.Age}\nGoal: {user.GoalType}\nLanguage: {user.Language}";
        await bot.SendTextMessageAsync(message.Chat.Id, info);
    }
}
