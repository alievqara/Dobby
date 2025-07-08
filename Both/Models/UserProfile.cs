using System.ComponentModel.DataAnnotations;

public class UserProfile
{
    [Key]
    public long TelegramUserId { get; set; } // bu primary key olmalı

    public int Step { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public string? GoalType { get; set; }

    public string? IncomeStatus { get; set; }

    public string? ReminderTime { get; set; }

    public string? Language { get; set; }
}
