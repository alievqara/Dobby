using Both.Data;
using Both.Models;
using Microsoft.EntityFrameworkCore;

namespace Both.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddTaskAsync(long userId, string description)
    {
        var task = new TaskItem { TelegramUserId = userId, Description = description };
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksAsync(long userId)
    {
        return await _context.Tasks.Where(t => t.TelegramUserId == userId).ToListAsync();
    }

    public async Task MarkDoneAsync(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task != null)
        {
            task.IsDone = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
