using Both.Services;
using Both.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Both.Handlers;

public class LanguageHandler
{
    private readonly IUserService _userService;

    public LanguageHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task HandleAsync(ITelegramBotClient bot, Message message)
    {
        var user = await _userService.GetOrCreateAsync(message.From!.Id);
        var text = message.Text?.Trim();
        if (text == "/language")
        {
            await bot.SendTextMessageAsync(message.Chat.Id, "Choose language", replyMarkup: new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("Русский"), new KeyboardButton("English"), new KeyboardButton("Türkçe") } }) { ResizeKeyboard = true, OneTimeKeyboard = true });
            user.Step = 6; // reuse onboarding step
        }
        else
        {
            user.Language = text switch
            {
                "Русский" => Language.RU,
                "Türkçe" => Language.TR,
                _ => Language.EN
            };
            await bot.SendTextMessageAsync(message.Chat.Id, $"Language set to {user.Language}");
            user.Step = 999;
        }
        await _userService.UpdateAsync(user);
    }
}
