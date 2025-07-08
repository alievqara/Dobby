using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Both.Models;

public class Expense
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("UserProfile")]
    public long TelegramUserId { get; set; }

    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
}
