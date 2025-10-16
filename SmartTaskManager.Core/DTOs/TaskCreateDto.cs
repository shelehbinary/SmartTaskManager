using System.ComponentModel.DataAnnotations;

namespace SmartTaskManager.Core.DTOs;

public class TaskCreateDto
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; }
}
