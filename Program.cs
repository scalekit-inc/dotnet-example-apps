var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


var app = builder.Build();

app.UseExceptionHandler("/error.html");
app.UseStatusCodePagesWithReExecute("/error.html");

// Enable static file serving
app.UseStaticFiles();

app.UseRouting();

// Add API routing here
app.MapControllers();

// For client-side routing in React (optional but common)
app.MapFallbackToFile("index.html");

app.Run();
