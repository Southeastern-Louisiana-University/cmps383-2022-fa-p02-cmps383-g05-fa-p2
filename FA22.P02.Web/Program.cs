using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

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


Product Sayan = new Product
{
    Id = 1,
    Name = "Yoo",
    Description = "Loo",
    Price = 1M
};
Product Saya = new Product
{
    Id = 2,
    Name = "Yoow",
    Description = "Loo",
    Price = 1M
};
Product Saan = new Product
{
    Id = 3,
    Name = "Yoao",
    Description = "Loo",
    Price = 1M
};




app.MapGet("/api/products/{id}", async (int id, ProductDb db) =>
{
    if (await db.Products.FindAsync(id) is Product find)
    {
        return Results.Ok(find);
    }
    return Results.NotFound("this is not the id you are looking for  - Obiwan Vidacovich");
}).WithName("Find a Specific Product");

app.MapPost("/api/products/update", async (Product idea, ProductDb db) =>
{
    if (await db.Products.FindAsync(idea.Id) is Product remove)
    {
        db.Products.Remove(remove);
        await db.SaveChangesAsync();
        db.Products.Add(idea);
        await db.SaveChangesAsync();
        return Results.Ok(idea);
    }
    return Results.BadRequest("this is not the id you are looking for  - Obiwan Vidacovich");
}).WithName("Update");

app.MapPut("/api/products/{id}", async (int id, Product pro, ProductDb db) =>
{
    var todo = await db.Products.FindAsync(id);

    if (todo is null) return Results.NotFound();

    if ((todo.Name.Length < 120) && (todo.Description != null) && (todo.Price != null) && (todo.Price > 0) && (todo.Name != null))
    {
        todo.Name = pro.Name;
        todo.Description = pro.Description;
        todo.Price = pro.Price;
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }




    return Results.BadRequest();
});


app.MapGet("/api/products", async (ProductDb db) =>
{ 
    if(!(await db.Products.FindAsync(Sayan.Id) is Product Nope))
    {
        db.Products.Add(Sayan);
        await db.SaveChangesAsync();
        db.Products.Add(Saan);
        await db.SaveChangesAsync();
        db.Products.Add(Saya);
        await db.SaveChangesAsync();
        
        return await db.Products.ToListAsync();
    }else
    {
        return await db.Products.ToListAsync();
    }
    
    
}).WithName("Get All Products");

app.MapPost("/api/products", async (Product prod, ProductDb db) =>
{
    if (((prod.Id > 0) && (prod.Name != null) && (prod.Description != null) && (prod.Price > 0M)) && !(await db.Products.FindAsync(prod.Id) is Product loose))
    {
        db.Products.Add(prod);
        await db.SaveChangesAsync();

        return Results.Created($"/products/{prod.Id}", prod);
    }
    return Results.BadRequest();
}).WithName("Create New Product");

app.MapDelete("/api/products", async (int id, ProductDb db) =>
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




public class TestBootstrapper
{
    /// <summary>
    /// Create an instance of in memory database context for testing.
    /// Use the returned DbContextOptions to initialize DbContext.
    /// </summary>
    /// <param name="dbName"></param>
    /// <returns></returns>
    public static DbContextOptions<DbContext> GetInMemoryDbContextOptions(string dbName = "Test_DB")
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return options;
    }
}
public class Product
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set;  }
}

public class ProductDb : DbContext
{
    /*public ProductDb(DbContextOptions<ProductDb> options)
    : base(options) { }
    */
    public ProductDb(DbContextOptions options) : base(options)
    {
    }

    //public DbSet<Product> Products { get; set; }
    public DbSet<Product> Products => Set<Product>();
}

//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Program { }