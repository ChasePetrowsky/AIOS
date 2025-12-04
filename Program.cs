using AIOS.Configuration;
using AIOS.Repositories;
using AIOS.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });


// Configure MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<NewsletterRepository>();


// Register Customer Repository as Singleton
builder.Services.AddSingleton<CustomerRepository>();

// Register Appointment Repository as Singleton
builder.Services.AddSingleton<AppointmentRepository>();

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

// Configure MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register Customer Repository as Singleton
builder.Services.AddSingleton<CustomerRepository>();

builder.Services.AddSingleton<AiPricingRepository>();


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