using ImageUploadAPI.Contexts;
using ImageUploadAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext con PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Configurar CORS para permitir solicitudes desde cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Añadir servicios para Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Habilitar CORS
app.UseCors("AllowAll");

// Verificar si la base de datos existe antes de iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!dbContext.Database.CanConnect())
    {
        throw new Exception("❌ ERROR: No se pudo conectar a la base de datos.\n" +
                            "🔍 Posible causa: La base de datos no existe o las credenciales son incorrectas.\n\n" +
                            "🔹 Solución: Ejecuta los siguientes comandos para aplicar las migraciones:\n" +
                            "   1️⃣ dotnet ef migrations add InitialCreate\n" +
                            "   2️⃣ dotnet ef database update");
    }
   
}

//abro el swagger en modo desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }
        await next();
    });
}

app.UseHttpsRedirection();

// Endpoint para subir imágenes
app.MapPost("/upload", async (IFormFile file, string name, int dorsal, AppDbContext db) =>
{
    if (file == null || file.Length == 0)
        return Results.BadRequest("No se proporcionó un archivo válido.");

    using var memoryStream = new MemoryStream();
    await file.CopyToAsync(memoryStream);
    var imageBytes = memoryStream.ToArray();

    var image = new ImageData { Name = name, Dorsal = dorsal, Data = imageBytes };
    db.Images.Add(image);
    await db.SaveChangesAsync();

    return Results.Ok(new { Message = "Imagen guardada exitosamente", image.Id });
})
.DisableAntiforgery();

// Endpoint para buscar imágenes por nombre o dorsal
app.MapGet("/search", async (string? name, int? dorsal, AppDbContext db) =>
{
    var query = db.Images.AsQueryable();

    if (!string.IsNullOrEmpty(name))
        query = query.Where(img => img.Name.Contains(name));
    if (dorsal.HasValue)
        query = query.Where(img => img.Dorsal == dorsal);

    var results = await query.Select(img => new { img.Id, img.Name, img.Dorsal, img.Data }).ToListAsync();

    return Results.Ok(results);
});


app.Run();
