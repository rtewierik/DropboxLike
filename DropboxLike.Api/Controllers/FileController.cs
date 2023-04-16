using DropboxLike.Domain.Configuration;
using DropboxLike.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DropboxLike.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
  private IFileService _fileService;
  private readonly AwsConfiguration _awsConfiguration;
  
  public FileController(AwsConfiguration awsConfiguration)
  {
    _awsConfiguration = awsConfiguration;
  }

  [HttpPost]
  [Route("Upload")]
  public async Task<IActionResult> UploadFileAsync(IFormFile file)
  {
    var fileExt = Path.GetExtension(file.Name);
    var objName = $"{Guid.NewGuid()}.{fileExt}";

		_fileService = new FileService((IFileService)_awsConfiguration);

    return Ok();
  }
}