namespace Both.Services;

public interface ITaskService
{
    Task AddTaskAsync(long userId, string description);
    Task<IEnumerable<TaskItem>> GetTasksAsync(long userId);
    Task MarkDoneAsync(int taskId);
    Task DeleteTaskAsync(int taskId);
}
