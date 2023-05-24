using DropboxLike.Domain.Models;
using DropboxLike.Domain.Repositories;
using DropboxLike.Domain.Services;
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
    
    // TODO: Find a way to return result with correct status code (in case of failure 404, 500, et cetera) using output of FileService call).

    return Ok(response.Successful);
  }

  [HttpGet]
  [Route("Download/{fileId}")]
  public async Task<IActionResult> DownloadFileAsync(string fileId)
  {
    var response = await _fileService.DownloadSingleFileAsync(fileId);
    try
    {
      if (response.Successful)
      {
        var file = response.Result;
        return new FileStreamResult(file.FileStream, file.ContentType);
      }
      else
      {
        return BadRequest($"Failed to read, {fileId}: {response.FailureMessage!}");
      }
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to read, {fileId}: {ex.Data}-{response.Exception}");
    }
  }

  [HttpDelete]
  [Route("Delete/{fileId}")]
  public async Task<IActionResult> DeleteFileAsync(string fileId)
  {
    var response = await _fileService.DeleteSingleFileAsync(fileId);
    
    // TODO: Find a way to return result with correct status code (in case of failure 404, 500, et cetera) using output of FileService call).
    
    return Ok(response);
  }
}