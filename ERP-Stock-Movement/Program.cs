using ERP_Stock_Movement.Inventory.Data;
using ERP_Stock_Movement.Inventory.Repositories;
using ERP_Stock_Movement.Inventory.Services;
using ERP_Stock_Movement.Orders.Clients;
using ERP_Stock_Movement.Orders.Data;
using ERP_Stock_Movement.Orders.Repositories;
using ERP_Stock_Movement.Orders.Services;
using ERP_Stock_Movement.Products.Data;
using ERP_Stock_Movement.Products.Repositories;
using ERP_Stock_Movement.Products.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var productConnection = builder.Configuration.GetConnectionString("ProductDb")
    ?? "Data Source=products.db";
var inventoryConnection = builder.Configuration.GetConnectionString("InventoryDb")
    ?? "Data Source=inventory.db";
var orderConnection = builder.Configuration.GetConnectionString("OrderDb")
    ?? "Data Source=orders.db";

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlite(productConnection));
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlite(inventoryConnection));
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlite(orderConnection));

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<InventoryRepository>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<OrderService>();

var moduleBaseUrl = builder.Configuration["ModuleCommunication:BaseUrl"] ?? "http://localhost:5094";

builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(client =>
{
    client.BaseAddress = new Uri(moduleBaseUrl);
});

builder.Services.AddHttpClient<IInventoryApiClient, InventoryApiClient>(client =>
{
    client.BaseAddress = new Uri(moduleBaseUrl);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var productDb = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    var inventoryDb = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    var orderDb = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

    if (app.Environment.IsDevelopment())
    {
        productDb.Database.EnsureDeleted();
        inventoryDb.Database.EnsureDeleted();
        orderDb.Database.EnsureDeleted();
    }

    productDb.Database.EnsureCreated();
    inventoryDb.Database.EnsureCreated();
    orderDb.Database.EnsureCreated();
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
