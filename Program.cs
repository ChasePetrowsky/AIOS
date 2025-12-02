using AIOS.Configuration;
using AIOS.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register Customer Repository as Singleton
builder.Services.AddSingleton<CustomerRepository>();

// Add CORS policy to allow frontend requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Test MongoDB connection
try
{
    var customerRepository = app.Services.GetRequiredService<CustomerRepository>();
    var customers = await customerRepository.GetAllAsync();
    Console.WriteLine($"? MongoDB connected successfully! Found {customers.Count} customer(s).");
}
catch (Exception ex)
{
    Console.WriteLine($"? MongoDB connection failed: {ex.Message}");
    Console.WriteLine("Please check your connection string in appsettings.json");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthorization();

// Map MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map API routes
app.MapControllers();

app.Run();