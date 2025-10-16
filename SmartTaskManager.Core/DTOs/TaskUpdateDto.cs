using System.ComponentModel.DataAnnotations;

namespace SmartTaskManager.Core.DTOs;

public class TaskUpdateDto
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }
}
