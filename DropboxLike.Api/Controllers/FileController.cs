using DropboxLike.Domain.Models;
using DropboxLike.Domain.Repositories;
using DropboxLike.Domain.Services;
using DropboxLike.Domain.Services.File;
using Microsoft.AspNetCore.Mvc;
using File = DropboxLike.Domain.Models.File;


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

    return StatusCode(response.StatusCode);
  }

  [HttpGet]
  [Route("Download/{fileId}")]
  public async Task<IActionResult> DownloadFileAsync(string fileId)
  {
    var response = await _fileService.DownloadSingleFileAsync(fileId);

    if (!response.IsSuccessful)
    {
      var message = $"Failed to download file with ID {fileId} due to '{response.FailureMessage ?? "<>"}'";
      return StatusCode(response.StatusCode, message);
    }
    var file = response.Value;
    return new FileStreamResult(file.FileStream, file.ContentType);
  }

  [HttpGet]
  [Route("List")]
  public async Task<IActionResult> ListFilesAsync()
  {
    var response = await _fileService.ListBucketFilesAsync();
    
    if (!response.IsSuccessful)
    {
      var message = $"Due to '{response.FailureMessage ?? "<>"}', your list was not loaded. Refresh your page!";
      return StatusCode(response.StatusCode, message);
    }
    return Ok(response.Result);
  }

  [HttpDelete]
  [Route("Delete/{fileId}")]
  public async Task<IActionResult> DeleteFileAsync(string fileId)
  {
    var response = await _fileService.DeleteSingleFileAsync(fileId);
    if (response.IsSuccessful) return NoContent();
    var message = $"Failed to delete file with ID {fileId} due to '{response.FailureMessage ?? "<>"}'";
    return StatusCode(response.StatusCode, message);
  }
}