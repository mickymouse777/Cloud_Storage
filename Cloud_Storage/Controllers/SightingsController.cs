using Cloud_Storage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Cloud_Storage.Services;

public class SightingsController : Controller
{
    private readonly TableStorageService _tableStorageService;
    private readonly QueueService _queueService;

    public SightingsController(TableStorageService tableStorageService, QueueService queueService)
    {
        _tableStorageService = tableStorageService;
        _queueService = queueService;
    }

    // Action to display all sightings (optional first)
    public async Task<IActionResult> Index()
    {
        var sightings = await _tableStorageService.GetAllSightingsAsync();
        return View(sightings);
    }
    public async Task<IActionResult> Register()
    {
        var birders = await _tableStorageService.GetAllBirdersAsync();
        var birds = await _tableStorageService.GetAllBirdsAsync();

        // Check for null or empty lists
        if (birders == null || birders.Count == 0)
        {
            // Handle the case where no birders are found
            ModelState.AddModelError("", "No birders found. Please add birders first.");
            return View(); // Or redirect to another action
        }

        if (birds == null || birds.Count == 0)
        {
            // Handle the case where no birds are found
            ModelState.AddModelError("", "No birds found. Please add birds first.");
            return View(); // Or redirect to another action
        }

        ViewData["Birders"] = birders;
        ViewData["Birds"] = birds;

        return View();
    }



    // Action to handle the form submission and register the sighting
    [HttpPost]
    public async Task<IActionResult> Register(Sighting sighting)
    {
        if (ModelState.IsValid)
        {//TableService
            sighting.Sighting_Date = DateTime.SpecifyKind(sighting.Sighting_Date, DateTimeKind.Utc);
            sighting.PartitionKey = "SightingsPartition";
            sighting.RowKey = Guid.NewGuid().ToString();
            await _tableStorageService.AddSightingAsync(sighting);
         //MessageQueue
            string message = $"New sighting by Birder {sighting.Birder_ID} of Bird {sighting.Bird_ID} at {sighting.Sighting_Location} on {sighting.Sighting_Date}";
            await _queueService.SendMessageAsync(message);

            return RedirectToAction("Index");
        }
        else
        {
            // Log model state errors
            foreach (var error in ModelState)
            {
                Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }
        }

        // Reload birders and birds lists if validation fails
        var birders = await _tableStorageService.GetAllBirdersAsync();
        var birds = await _tableStorageService.GetAllBirdsAsync();
        ViewData["Birders"] = birders;
        ViewData["Birds"] = birds;

        return View(sighting);
    }

}

