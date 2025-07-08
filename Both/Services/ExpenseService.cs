using Both.Data;
using Both.Models;
using Microsoft.EntityFrameworkCore;

namespace Both.Services;

public class ExpenseService : IExpenseService
{
    private readonly AppDbContext _context;

    public ExpenseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddExpenseAsync(long userId, decimal amount, string category)
    {
        var exp = new Expense { TelegramUserId = userId, Amount = amount, Category = category };
        _context.Expenses.Add(exp);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesAsync(long userId)
    {
        return await _context.Expenses.Where(e => e.TelegramUserId == userId).ToListAsync();
    }
}
