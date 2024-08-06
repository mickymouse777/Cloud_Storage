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

        bird.PartitionKey = "BirdsPartition";
        bird.RowKey = Guid.NewGuid().ToString();

        await _tableStorageService.AddBirdAsync(bird);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBird(string partitionKey, string rowKey)
    {

        await _tableStorageService.DeleteBirdAsync(partitionKey, rowKey);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index()
    {
        var birds = await _tableStorageService.GetAllBirdsAsync();
        return View(birds);
    }


}
