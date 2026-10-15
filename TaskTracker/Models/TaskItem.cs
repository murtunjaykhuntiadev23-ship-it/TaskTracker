using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = "";

    public string? Description { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}