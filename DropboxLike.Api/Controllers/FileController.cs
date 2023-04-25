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
    var response = await _fileService.UploadSingleFileAsync(file);

    return Ok(response);
  }

  [HttpGet]
  [Route("Download")]
  public async Task<IActionResult> DownloadFileAsync(string request)
  {
    var response = await _fileService.DownloadSingleFileAsync(request);

    return File(response.Content, response.ContentType, response.OriginalFileName);
  }
}