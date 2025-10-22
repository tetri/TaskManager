using FluentMigrator.Runner;

using TaskManager.Data;
using TaskManager.Migrations;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adicionar FluentMigrator e Repositório
builder.Services.AddFluentMigrator(connectionString);
builder.Services.AddScoped(_ => new TaskRepository(connectionString));

var corsPolicyName = "AllowAllOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(corsPolicyName);

// Executar migrations ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    migrator.MigrateUp();

    var repo = scope.ServiceProvider.GetRequiredService<TaskRepository>();
    var tasks = await repo.GetAllAsync();

    if (!tasks.Any())
    {
        for (int i = 1; i <= 60; i++)
        {
            var task = new TaskManager.Models.Task
            {
                Title = $"Task {i}",
                Description = $"Description for Task {i}",
                DueDate = DateTime.Now.AddDays(i),
                IsCompleted = i % 2 == 0
            };

            await repo.AddAsync(task);
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
