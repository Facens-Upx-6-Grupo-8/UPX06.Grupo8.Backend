using Application.DependencyInjection;
using Application.Persistance;
using Domain.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddCors(options => options.AddPolicy("Frontend", policy =>
    policy.WithOrigins("http://localhost:5173")
        .WithMethods("GET", "POST")
        .WithHeaders("Accept", "Content-Type")));
builder.Services.AddOpenApi();
builder.Services.AddApplication($"Data Source={Path.Combine(builder.Environment.ContentRootPath, ".db")}");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.Set<SampleEvaluator>().Any())
    {
        dbContext.Set<SampleEvaluator>().Add(new SampleEvaluator(
            PHSettings: new SampleMetricEvaluator(6.5, 9.5, true),
            TurbiditySettings: new SampleMetricEvaluator(0, 1, true),
            TemperatureSettings: new SampleMetricEvaluator(20, 30, true),
            TDSSettings: new SampleMetricEvaluator(0, 600, true)
        ));

        dbContext.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}
app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
