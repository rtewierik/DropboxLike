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
  [Route("Download/{fileId}")]
  public async Task<IActionResult> DownloadFileAsync(string fileId)
  {
    // var destinationFolderPath = $"/home/godfreyowidi/Downloads/TestsDowloads";

    var file = await _fileService.DownloadSingleFileAsync(fileId);

    return Ok();
  }

  [HttpDelete]
  [Route("Delete/{fileId}")]
  public async Task<IActionResult> DeleteFileAsync(string fileId)
  {
    var response = await _fileService.DeleteSingleFileAsync(fileId);
    return Ok(response);
  }
}