using DockerTest.Data;
using DockerTest.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// SQLite-Datenbank registrieren
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")
    )
);

var app = builder.Build();

// SQLite-Datenbank automatisch erstellen
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    db.Database.EnsureCreated();
}

// POST /api/orders
app.MapPost("/api/orders", async (
    CreateOrderRequest request,
    AppDbContext db) =>
{
    // Eingaben überprüfen
    if (string.IsNullOrWhiteSpace(request.CustomerEmail) ||
        string.IsNullOrWhiteSpace(request.Item) ||
        request.Price < 0)
    {
        return Results.BadRequest(
            "Ungültige Bestelldaten.");
    }

    // Neue Bestellung erstellen
    var order = new Order
    {
        CustomerEmail = request.CustomerEmail,
        Item = request.Item,
        Price = request.Price,
        CreatedAt = DateTime.UtcNow,
        Status = "Pending"
    };

    // Bestellung hinzufügen
    db.Orders.Add(order);

    // In SQLite speichern
    await db.SaveChangesAsync();

    // Antwort zurückgeben
    return Results.Created(
        $"/api/orders/{order.Id}", order);
});

app.Run();
