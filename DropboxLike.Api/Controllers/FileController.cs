using DropboxLike.Domain.Configuration;
using DropboxLike.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DropboxLike.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
  private readonly IFileService _fileService;
  
  public FileController(IFileService fileService)
  {
    _fileService = fileService;
  }

  [HttpPost]
  [Route("Upload")]
  public async Task<IActionResult> UploadFileAsync(IFormFile file)
  {
    var fileExt = Path.GetExtension(file.Name);
    var objName = $"{Guid.NewGuid()}.{fileExt}";

    var response = _fileService.UploadSingleFile(file);

    return Ok(response);
  }
}