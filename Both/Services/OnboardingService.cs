using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Both.Services
{
    public class OnboardingService
    {
        private readonly IUserService _userService;

        public OnboardingService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleStepAsync(ITelegramBotClient bot, Message message, UserProfile user)
        {
            var chatId = message.Chat.Id;
            var text = message.Text?.Trim();

            switch (user.Step)
            {
                case 0:
                    await bot.SendMessage(chatId,
                        $"Привет, {message.From?.FirstName ?? "друг"}! Меня зовут Dobby 🤖\n" +
                        "Я твой персональный ассистент.\n\n➡️ Как тебя зовут?");
                    user.Step = 1;
                    break;

                case 1:
                    user.Name = text;
                    await bot.SendMessage(chatId, "📆 Сколько тебе лет?", replyMarkup: GetNumberKeyboard());
                    user.Step = 2;
                    break;

                case 2:
                    if (int.TryParse(text, out int age))
                    {
                        user.Age = age;
                        await bot.SendMessage(chatId,
                            "💡 Как ты хочешь использовать помощника?\n" +
                            "1. 📅 Ежедневное планирование\n2. 📈 Финансовый учёт\n3. 🧠 Личное развитие\n4. 📊 Всё вместе");
                        user.Step = 3;
                    }
                    else
                    {
                        await bot.SendMessage(chatId, "❌ Введи возраст числом.");
                    }
                    break;

                case 3:
                    user.GoalType = text;
                    await bot.SendMessage(chatId,
                        "💰 У тебя стабильный доход?\n1. Да\n2. Нет\n3. Пока нет дохода");
                    user.Step = 4;
                    break;

                case 4:
                    user.IncomeStatus = text;
                    await bot.SendMessage(chatId,
                        "⏰ В какое время тебе удобно получать напоминания? (например: 08:00)");
                    user.Step = 5;
                    break;

                case 5:
                    user.ReminderTime = text;
                    await bot.SendMessage(chatId,
                        "🌍 Выбери язык (язык можно изменить позже)",
                        replyMarkup: GetLanguageKeyboard());
                    user.Step = 6;
                    break;

                case 6:
                    user.Language = text;
                    await bot.SendMessage(chatId,
                        $"✅ Готово, {user.Name}!\nЯ запомнил всё и готов помогать 🧠");
                    user.Step = 999;
                    break;

                default:
                    await bot.SendMessage(chatId,
                        $"👋 Привет снова, {user.Name}! Напиши /help чтобы продолжить.");
                    break;
            }

            await _userService.UpdateAsync(user);
        }

        private ReplyKeyboardMarkup GetNumberKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("18"), new KeyboardButton("25"), new KeyboardButton("30") },
                new[] { new KeyboardButton("35"), new KeyboardButton("40"), new KeyboardButton("50") }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }

        private ReplyKeyboardMarkup GetLanguageKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("Русский"), new KeyboardButton("English"), new KeyboardButton("Türkçe") }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
    }
}
