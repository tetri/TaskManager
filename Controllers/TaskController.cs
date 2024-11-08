using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskRepository _repository;

    public TasksController(TaskRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _repository.GetAllAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
            return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Models.Task task)
    {
        await _repository.AddAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Models.Task task)
    {
        task.Id = id;
        var result = await _repository.UpdateAsync(task);
        if (result == 0)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (result == 0)
            return NotFound();
        return NoContent();
    }
}
