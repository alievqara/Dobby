using Both.Models;

namespace Both.Services;

public class SummaryService
{
    private readonly ITaskService _taskService;
    private readonly IExpenseService _expenseService;

    public SummaryService(ITaskService taskService, IExpenseService expenseService)
    {
        _taskService = taskService;
        _expenseService = expenseService;
    }

    public async Task<string> GetSummaryAsync(long userId)
    {
        var tasks = await _taskService.GetTasksAsync(userId);
        var expenses = await _expenseService.GetExpensesAsync(userId);
        var pending = tasks.Count(t => !t.IsDone);
        var spent = expenses.Sum(e => e.Amount);

        return $"Pending tasks: {pending}\nTotal spent: {spent}";
    }
}
