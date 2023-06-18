using System.Text;
using DropboxLike.Domain.Configuration;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var region = Environment.GetEnvironmentVariable("AWS_REGION");
var secret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
var access = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");

Console.WriteLine("AWS_REGION: " + region);
Console.WriteLine("AWS_SECRET: " + secret);
Console.WriteLine("AWS_KEY: " + access);


// 1. Add configuration.
builder.Services.Configure<AwsConfiguration>(options =>
{
    options.BucketName = builder.Configuration.GetSection("Aws:BucketName").Get<string>() ?? string.Empty;
    options.AwsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY") ?? string.Empty;
    options.AwsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? string.Empty;
    options.Region = Environment.GetEnvironmentVariable("AWS_REGION") ?? string.Empty;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DropboxLikeConn"),
    sqlServerOptionsAction: sqlOptions => 
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        );
    }));
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 2. Add lowest layer components, namely repositories.
builder.Services.AddScoped<DropboxLike.Domain.Repositories.IFileRepository, DropboxLike.Domain.Repositories.FileRepository>();

// 3. Add higher layer components, namely services.
builder.Services.AddScoped<DropboxLike.Domain.Services.File.IFileService, DropboxLike.Domain.Services.File.FileService>();

// 4. Add even higher layer components, namely controllers and the related authorization and authentication.
builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();