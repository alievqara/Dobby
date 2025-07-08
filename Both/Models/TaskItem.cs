using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Both.Models;

public class TaskItem
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("UserProfile")]
    public long TelegramUserId { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    public bool IsDone { get; set; }
}
