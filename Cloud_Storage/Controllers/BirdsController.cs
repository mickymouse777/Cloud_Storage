using Cloud_Storage.Models;
using Cloud_Storage.Services;
using Microsoft.AspNetCore.Mvc;

public class BirdsController : Controller
{
    private readonly BlobService _blobService;
    private readonly TableStorageService _tableStorageService;

    public BirdsController(BlobService blobService, TableStorageService tableStorageService)
    {
        _blobService = blobService;
        _tableStorageService = tableStorageService;
    }

    [HttpGet]
    public IActionResult AddBird()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddBird(Bird bird, IFormFile file)
    {
        if (file != null)
        {
            using var stream = file.OpenReadStream();
            var imageUrl = await _blobService.UploadAsync(stream, file.FileName);
            bird.ImageUrl = imageUrl;
        }

        if (ModelState.IsValid)
        {
            bird.PartitionKey = "BirdsPartition";
            bird.RowKey = Guid.NewGuid().ToString();
            await _tableStorageService.AddBirdAsync(bird);
            return RedirectToAction("Index");
        }
        return View(bird);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBird(string partitionKey, string rowKey, Bird bird )
    {
         
        if (bird != null && !string.IsNullOrEmpty(bird.ImageUrl))
        {
            // Delete the associated blob image
            await _blobService.DeleteBlobAsync(bird.ImageUrl);
        }
        //Delete Table entity
        await _tableStorageService.DeleteBirdAsync(partitionKey, rowKey);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index()
    {
        var birds = await _tableStorageService.GetAllBirdsAsync();
        return View(birds);
    }
        
}
