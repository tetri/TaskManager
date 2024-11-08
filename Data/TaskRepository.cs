using Dapper;
using Microsoft.Data.Sqlite;
using TaskManager.Models;

namespace TaskManager.Data;

public class TaskRepository
{
    private readonly string _connectionString;

    public TaskRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqliteConnection GetConnection() => new SqliteConnection(_connectionString);

    public async Task<IEnumerable<Models.Task>> GetAllAsync()
    {
        using var connection = GetConnection();
        return await connection.QueryAsync<Models.Task>("SELECT * FROM Tasks");
    }

    public async Task<Models.Task> GetByIdAsync(int id)
    {
        using var connection = GetConnection();
        return await connection.QuerySingleOrDefaultAsync<Models.Task>("SELECT * FROM Tasks WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> AddAsync(Models.Task task)
    {
        using var connection = GetConnection();
        return await connection.ExecuteAsync(
            "INSERT INTO Tasks (Title, Description, DueDate, IsCompleted) VALUES (@Title, @Description, @DueDate, @IsCompleted)",
            task);
    }

    public async Task<int> UpdateAsync(Models.Task task)
    {
        using var connection = GetConnection();
        return await connection.ExecuteAsync(
            "UPDATE Tasks SET Title = @Title, Description = @Description, DueDate = @DueDate, IsCompleted = @IsCompleted WHERE Id = @Id",
            task);
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = GetConnection();
        return await connection.ExecuteAsync("DELETE FROM Tasks WHERE Id = @Id", new { Id = id });
    }
}
