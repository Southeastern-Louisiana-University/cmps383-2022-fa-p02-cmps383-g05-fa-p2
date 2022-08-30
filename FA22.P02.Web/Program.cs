using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProductDb>(opt => opt.UseInMemoryDatabase("ProductList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/api/productsById", async (int id, ProductDb db) =>
{
    if (await db.Products.FindAsync(id) is Product find)
    {
        return Results.Ok(find);
    }
    return Results.NotFound("this is not the id you are looking for  - Obiwan Vidacovich");
}).WithName("Find a Specific Product");


app.MapGet("/api/products", async (ProductDb db) =>
    await db.Products.ToListAsync())
    .WithName("Get All Products");

app.MapPost("/api/addProduct", async (Product prod, ProductDb db) =>
{
    db.Products.Add(prod);
    await db.SaveChangesAsync();

    return Results.Created($"/products/{prod.Id}", prod);
}).WithName("Create New Product");

app.MapDelete("/api/removeProduct", async (int id, ProductDb db) =>
{
    if (await db.Products.FindAsync(id) is Product remove)
    {
        db.Products.Remove(remove);
        await db.SaveChangesAsync();
        return Results.Ok(remove);
    }
    return Results.BadRequest("this is not the id you are looking for  - Obiwan Vidacovich");
}).WithName("Delete Product");


app.Run();


class Product
{

    public int Id { get; set; }
    public string Name { get; set; }
    
    public string? Description { get; set; }

    public decimal Price { get; set;  }
}

class ProductDb : DbContext
{
    public ProductDb(DbContextOptions<ProductDb> options)
    : base(options) { }

    public DbSet<Product> Products => Set<Product>();
}

//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Program { }