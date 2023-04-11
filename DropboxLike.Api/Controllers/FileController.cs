using Amazon.S3;
using DropboxLike.Domain.Contracts;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Models;
using DropboxLike.Domain.Repositors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DropboxLike.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Filecontroller : ControllerBase
{
  private readonly IFileRepository _fileRepository;
  private readonly IConfiguration _config;
  public Filecontroller(IFileRepository fileRepository, IConfiguration config)
  {
    _fileRepository = fileRepository;
    _config = config;
  }

  [HttpPost]
  [Route("Upload")]
  public async Task<IActionResult> UploadFileAsync(IFormFile file)
  {
    await using var target = new MemoryStream();
    await file.CopyToAsync(target);

    var fileExt = Path.GetExtension(file.Name);
    var objName = $"{Guid.NewGuid()}.{fileExt}";

    var s3Obj = new FileObject()
    {
      BucketName = "dropboxlike",
      InputStream = target,
      Name = objName
    };

    var cred = new AwsCredentials()
    {
      AwsKey = _config["AwsConfiguration:AWSAccessKey"],
      AwsSecretKey = _config["AwsConfiguration:AWSSecretKey"]
    };

    var res = await _fileRepository.UploadFileAsync(s3Obj, cred);

    return Ok(res);
  }
}