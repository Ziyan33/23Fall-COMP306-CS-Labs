using BookResearcher.Domain.Data;
using BookResearcher.Domain.Interfaces;
using BookResearcher.Domain.Models;
using BookResearcher.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<BookResearcherContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookResearcherDb")));

/*
builder.Services.AddDbContext<BookResearcherContext>(options =>
options.UseSqlServer(
        builder.Configuration.GetConnectionString("BookResearcherDb"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // The number of times to retry before failing
                maxRetryDelay: TimeSpan.FromSeconds(30), // The maximum delay between retries
                errorNumbersToAdd: null); // SQL error numbers to consider as transient
        }));
*/
// Register your repositories here
builder.Services.AddScoped<IRepository<Book>, BookRepository>();
builder.Services.AddScoped<IRepository<Author>, AuthorRepository>();
builder.Services.AddScoped<IRepository<LibraryBranch>, LibraryBranchRepository>();
builder.Services.AddScoped<IRepository<BookAuthors>, BookAuthorsRepository>();
builder.Services.AddScoped<IRepository<BookAvailability>, BookAvailabilityRepository>();




//MappingProfile in BookResearcher.Domain
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        // Configure the JSON serializer options here, if needed
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

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

app.UseAuthorization();

app.MapControllers();

app.Run();
