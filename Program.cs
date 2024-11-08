using TaskManager.Data;
using TaskManager.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Adicionar FluentMigrator e Repositório
builder.Services.AddFluentMigrator(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddScoped(_ => new TaskRepository(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Executar migrations ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    migrator.MigrateUp();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
