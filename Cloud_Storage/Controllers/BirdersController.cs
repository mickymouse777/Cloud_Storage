using Cloud_Storage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class BirdersController : Controller
{
    private readonly TableStorageService _tableStorageService;

    public BirdersController(TableStorageService tableStorageService)
    {
        _tableStorageService = tableStorageService;
    }

    public async Task<IActionResult> Index()
    {
        var birders = await _tableStorageService.GetAllBirdersAsync();
        return View(birders);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Birder birder)
    {
        birder.PartitionKey = "BirdersPartition";
        birder.RowKey = Guid.NewGuid().ToString();

        await _tableStorageService.AddBirderAsync(birder);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(string partitionKey, string rowKey)
    {
        await _tableStorageService.DeleteBirderAsync(partitionKey, rowKey);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(string partitionKey, string rowKey)
    {
        var birder = await _tableStorageService.GetBirderAsync(partitionKey, rowKey);
        if (birder == null)
        {
            return NotFound();
        }
        return View(birder);
    }
}
