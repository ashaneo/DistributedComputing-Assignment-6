using Microsoft.EntityFrameworkCore;
using DataTierAPI.Data;
using DataTierAPI.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQLite Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=DataRecords.db"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed Database (for development only)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated(); 
        // Seed the database if empty
    if (!context.DataRecords.Any())
    {
        context.DataRecords.AddRange(
            new DataIntermed { Bal = 1000, Acct = 12345678, Pin = 1234, FName = "John", LName = "Doe" },
            new DataIntermed { Bal = 2500, Acct = 87654321, Pin = 4321, FName = "Jane", LName = "Smith" },
            new DataIntermed { Bal = 500, Acct = 13579246, Pin = 5678, FName = "Alice", LName = "Johnson" }
        );
        context.SaveChanges();
    }
}

app.Run();
