using Cloud_Storage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FilesController : Controller
{
    private readonly AzureFileShareService _fileShareService;

    public FilesController(AzureFileShareService fileShareService)
    {
        _fileShareService = fileShareService;
    }

    // Action to list files in a directory
    [HttpGet]
    public async Task<IActionResult> ListFiles()
    {
        List<FileModel> files;
        try
        {
            files = await _fileShareService.ListFilesAsync("uploads");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to list files: {ex.Message}");
        }

        return View(files);
    }
}
