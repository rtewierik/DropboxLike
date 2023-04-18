using DropboxLike.Domain.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .AddEnvironmentVariables()
        .Build();

builder.Services.Configure<AwsConfiguration>(configuration.GetSection("AWS_BUCKET_NAME"));

// 1. Add configuration.
builder.Services.Configure<AwsConfiguration>(options =>
{
    // options.BucketName = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME") ?? string.Empty;
    options.AwsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY") ?? string.Empty;
    options.AwsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? string.Empty;
    options.Region = Environment.GetEnvironmentVariable("AWS_REGION") ?? string.Empty;
});

// 2. Add lowest layer components, namely repositories.
builder.Services.AddScoped<DropboxLike.Domain.Repositories.IFileRepository, DropboxLike.Domain.Repositories.FileRepository>();

// 3. Add higher layer components, namely services.
builder.Services.AddScoped<DropboxLike.Domain.Services.IFileService, DropboxLike.Domain.Services.FileService>();

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

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();