using Microsoft.EntityFrameworkCore;
using Helpdesk.Api.Data;
using Helpdesk.Api.Middleware;
using Helpdesk.Api.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ==============================================================================
// 1. Logging Configuration (Serilog)
// ==============================================================================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ==============================================================================
// 2. Services Registration
// ==============================================================================
builder.Services.AddControllers();

// Configure CORS policy for frontend client integration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Entity Framework with PostgreSQL provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// API Explorer & Swagger Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation Integration
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<TicketValidator>();

var app = builder.Build();

// ==============================================================================
// 3. HTTP Request Pipeline Middleware Configuration
// ==============================================================================
app.UseCors("AllowAll");

// Register global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Apply database migrations and seed initial data on application startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // Automatically apply pending EF Core migrations
        context.Database.Migrate();

        // Seed initial demo tickets asynchronously
        await DbInitializer.InitializeAsync(context);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while initializing or seeding the database.");
    }
}

// Enable Swagger UI only in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// ==============================================================================
// 4. Application Startup Execution
// ==============================================================================
try
{
    Log.Information("Starting Helpdesk API services...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly due to a fatal exception!");
}
finally
{
    Log.CloseAndFlush();
}