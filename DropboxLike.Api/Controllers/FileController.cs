using DropboxLike.Domain.Services.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DropboxLike.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FileController : BaseController
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
    var claims = GetClaims(); // TODO: Extract user ID from claims to create file FOR THAT USER.
    
    var response = await _fileService.UploadSingleFileAsync(file);

    return StatusCode(response.StatusCode);
  }

  [HttpGet]
  [Route("Download/{fileId}")]
  public async Task<IActionResult> DownloadFileAsync(string fileId)
  {
    var claims = GetClaims(); // TODO: Extract user ID from claims to download file OWNED BY THAT USER.
    
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
    var claims = GetClaims(); // TODO: Extract user ID from claims to list files OWNED BY THAT USER.
    
    var response = await _fileService.ListBucketFilesAsync();
    
    return Ok(response);
  }

  [HttpDelete]
  [Route("Delete/{fileId}")]
  public async Task<IActionResult> DeleteFileAsync(string fileId)
  {
    var claims = GetClaims(); // TODO: Extract user ID from claims to delete file OWNED BY THAT USER.
    
    var response = await _fileService.DeleteSingleFileAsync(fileId);
    if (response.IsSuccessful) return NoContent();
    var message = $"Failed to delete file with ID {fileId} due to '{response.FailureMessage ?? "<>"}'";
    return StatusCode(response.StatusCode, message);
  }
}