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
    
    // TODO: Find a way to return result with correct status code (in case of failure 404, 500, et cetera) using output of FileService call).

    return Ok(response);
  }

  [HttpGet]
  [Route("Download/{fileId}")]
  public async Task<IActionResult> DownloadFileAsync(string fileId)
  {
    // var destinationFolderPath = $"/home/godfreyowidi/Downloads/TestsDowloads";

    var result = await _fileService.DownloadSingleFileAsync(fileId);
    if (result.Successful)
    {
      var file = result.Result!;
      // TODO: Consume this result when returning some response.
      var fileStreamResult = new FileStreamResult(file.FileStream, file.ContentType);
      return Ok();
    }
    // TODO: Here we have to return some failure response indicating our download operation failed.
    var exception = result.Exception; // TODO: Be careful! Potentially null!
    var message = result.FailureMessage!;
    // If message is "500: Some internal error occurred", we want to return response with status code 500 and response body "Some internal error occurred".
    return Ok();
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