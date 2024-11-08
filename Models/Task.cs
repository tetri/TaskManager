namespace TaskManager.Models;

public class Task
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public required bool IsCompleted { get; set; } = false;

    public Task() { }
}
