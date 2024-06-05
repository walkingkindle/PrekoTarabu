using PrekoTarabu.Server.Models;
using PrekoTarabu.Server.Services;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MailerService>();


var connectionStringBuilder = new SqlConnectionStringBuilder(builder.Configuration.
    GetConnectionString("connectionString"));
connectionStringBuilder.Password = builder.Configuration["Credentials:DbPassword"];
var connection = connectionStringBuilder.ConnectionString;
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connection);
});

builder.Services.AddCors(options => options.AddPolicy(name:"frontend", policy => {policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();}));



var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("frontend");



app.MapControllers();

app.Run();
