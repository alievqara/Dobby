using Telegram.Bot;
using Telegram.Bot.Types;
using Both.Services;

namespace Both.Handlers
{
    public class StartHandler
    {
        private readonly IUserService _userService;
        private readonly OnboardingService _onboarding;

        public StartHandler(IUserService userService, OnboardingService onboarding)
        {
            _userService = userService;
            _onboarding = onboarding;
        }

        public async Task HandleAsync(ITelegramBotClient bot, Message message)
        {
            var user = await _userService.GetOrCreateAsync(message.From!.Id);

            await _onboarding.HandleStepAsync(bot, message, user);
        }
    }
}
