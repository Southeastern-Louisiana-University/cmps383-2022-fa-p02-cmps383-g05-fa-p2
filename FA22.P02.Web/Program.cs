var builder = WebApplication.CreateBuilder(args);

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


app.MapGet("/products", () =>
{

}
)
.WithName("GetAllProducts");

new ProductDto(1, "DillPickle", "Reg ol' pickle", 1.99M);
new ProductDto(2, "DillyPickle", "A very Dilly Pickle", 2.99M);
new ProductDto(3, "DillPickley", "A very Pickley Dill", 100.99M);

app.Run();

internal record ProductDto(int productId, string productName, string? Description, decimal productPrice)
{
    public int Id = productId;
    public string Name = productName;
    public string? Desc = Description;
    public decimal Price = productPrice;

}

//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Program { }