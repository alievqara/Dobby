namespace Both.Services;

public interface IExpenseService
{
    Task AddExpenseAsync(long userId, decimal amount, string category);
    Task<IEnumerable<Expense>> GetExpensesAsync(long userId);
}
