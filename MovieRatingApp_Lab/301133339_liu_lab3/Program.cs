using _301133339_liu_lab3.Data;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adding Identity services
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<UserManager<IdentityUser>>()
    .AddSignInManager<SignInManager<IdentityUser>>();

// Setup AWS configuration
var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Region = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]);
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var awsCredentials = new BasicAWSCredentials(
        builder.Configuration["AWS:AccessKey"],
        builder.Configuration["AWS:SecretKey"]);
    var region = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]);
    return new AmazonDynamoDBClient(awsCredentials, region);
});
builder.Services.AddAWSService<IAmazonS3>();

var app = builder.Build();

/*// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Make sure to include this line
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();