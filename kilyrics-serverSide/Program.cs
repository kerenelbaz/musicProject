//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (true)
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); //allow access 
//app.UseAuthorization();

//app.MapControllers();

//app.Run();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use environment-specific port if available
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}"); // Listen on the specified port

// Optional: Disable HTTPS redirection if not applicable in certain environments
if (!builder.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); // Allow cross-origin requests
app.UseAuthorization();

app.MapControllers();

app.Run();
