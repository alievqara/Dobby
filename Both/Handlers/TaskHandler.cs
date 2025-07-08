using Both.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Both.Handlers;

public class TaskHandler
{
    private readonly ITaskService _taskService;
    private readonly IUserService _userService;

    public TaskHandler(ITaskService taskService, IUserService userService)
    {
        _taskService = taskService;
        _userService = userService;
    }

    public async Task HandleAsync(ITelegramBotClient bot, Message message)
    {
        var text = message.Text?.Trim() ?? string.Empty;
        var parts = text.Split(' ', 2);
        var command = parts[0];
        var user = await _userService.GetOrCreateAsync(message.From!.Id);

        if (command == "/task" && parts.Length > 1)
        {
            await _taskService.AddTaskAsync(user.TelegramUserId, parts[1]);
            await bot.SendTextMessageAsync(message.Chat.Id, "Task added");
        }
        else if (command == "/tasks")
        {
            var tasks = await _taskService.GetTasksAsync(user.TelegramUserId);
            var lines = tasks.Select(t => $"{t.Id}. {(t.IsDone ? "✅" : "❌")} {t.Description}");
            await bot.SendTextMessageAsync(message.Chat.Id, string.Join('\n', lines));
        }
        else if (command == "/done" && parts.Length > 1 && int.TryParse(parts[1], out int id))
        {
            await _taskService.MarkDoneAsync(id);
            await bot.SendTextMessageAsync(message.Chat.Id, "Marked done");
        }
        else if (command == "/delete" && parts.Length > 1 && int.TryParse(parts[1], out id))
        {
            await _taskService.DeleteTaskAsync(id);
            await bot.SendTextMessageAsync(message.Chat.Id, "Deleted");
        }
    }
}
