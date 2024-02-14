using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using N5ChallengeWebApi.Models;
using N5ChallengeWebApi.Repository;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<N5UsersPermissionsContext>(options => options.UseSqlServer(builder.Configuration["ConnectionDB"]));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

// Elasticsearch connection
var pool = new SingleNodeConnectionPool(new Uri(builder.Configuration["ElasticSearchUrl"]));
var settings = new ConnectionSettings(pool).DefaultIndex("permission-index");
var client = new ElasticClient(settings);

builder.Services.AddSingleton<IElasticClient>(client);
builder.Services.AddSingleton<IPermissionElasticSearchRepository, PermissionElasticSearchRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
