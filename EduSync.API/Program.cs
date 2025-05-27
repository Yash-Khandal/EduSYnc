// EduSync/Program.cs (Backend Project)

using Microsoft.EntityFrameworkCore;
using EduSync.Data;

var builder = WebApplication.CreateBuilder(args);

// Define a specific CORS policy name
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // You can name this anything

// 1. Configure DbContext (Existing code)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EduSyncDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Add CORS services and define a policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000") // Your frontend's origin
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services to the container. (Existing code)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<EduSync.Services.BlobStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3. Use the CORS policy - IMPORTANT: Call UseCors before UseAuthorization and MapControllers
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization(); // This should come after UseCors

app.MapControllers();

app.Run();