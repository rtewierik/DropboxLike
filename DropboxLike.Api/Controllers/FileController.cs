using DropboxLike.Domain.Contracts;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Models;
using DropboxLike.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace DropboxLike.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Filecontroller : ControllerBase
{
  // private readonly IFileRepository _fileRepository;
  private readonly IAwsConfiguration _awsConfiguation;
  private IFileStore _fileStore;
  private readonly IConfiguration _config;
  public Filecontroller(IAwsConfiguration awsConfiguration, IConfiguration config)
  {
    _awsConfiguation = awsConfiguration;
    _config = config;
  }

  [HttpPost]
  [Route("Upload")]
  public async Task<IActionResult> UploadFileAsync(IFormFile file)
  {
    var fileExt = Path.GetExtension(file.Name);
    var objName = $"{Guid.NewGuid()}.{fileExt}";

    _fileStore = new FileStore((Domain.Repositors.AwsConfiguration)_awsConfiguation);

    var res = _fileStore.UploadSingleFile(file);

    return Ok(res);
  }
}