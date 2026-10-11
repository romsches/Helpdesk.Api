using Microsoft.EntityFrameworkCore;
using Helpdesk.Api.Data;
using Helpdesk.Api.Middleware;
using Helpdesk.Api.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Настраиваем Serilog для логирования
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Добавляем сервисы
builder.Services.AddControllers();

// Настройка CORS для подключения фронтенда
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<TicketValidator>();

var app = builder.Build();

// 3. Middleware пайплайн
app.UseCors("AllowAll");

// Обработка исключений
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Применение миграций и Seed Data при старте приложения
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // Автоматически применяем миграции и создаем таблицы в БД
        context.Database.Migrate();

        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Ein Fehler ist beim Initialisieren der Datenbank aufgetreten.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Запуск Helpdesk API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Приложение неожиданно остановилось из-за ошибки!");
}
finally
{
    Log.CloseAndFlush();
}