using DropboxLike.Domain.Configuration;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Middlewares;
using DropboxLike.Domain.Repositories.File;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// 2. Add lowest layer components, namely repositories.
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services
    .AddScoped<DropboxLike.Domain.Repositories.User.IUserRepository,
        DropboxLike.Domain.Repositories.User.UserRepository>();

// 3. Add higher layer components, namely services.
builder.Services.AddScoped<DropboxLike.Domain.Services.File.IFileService, DropboxLike.Domain.Services.File.FileService>();
builder.Services
    .AddScoped<DropboxLike.Domain.Services.User.IUserService, DropboxLike.Domain.Services.User.UserService>();

// 4. Add even higher layer components, namely controllers and the related authorization and authentication.
builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();

app.UseMiddleware<TokenvalidationMiddleware>();

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