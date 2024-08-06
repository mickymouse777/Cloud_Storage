using Azure;
using Azure.Data.Tables;
using Cloud_Storage.Models;
using System.Threading.Tasks;

public class TableStorageService
{
    private readonly TableClient _tableClient;

    public TableStorageService(string connectionString)
    {
        _tableClient = new TableClient(connectionString, "Birds");
    }

    public async Task<List<Bird>> GetAllBirdsAsync()
    {
        var birds = new List<Bird>();

        await foreach (var bird in _tableClient.QueryAsync<Bird>())
        {
            birds.Add(bird);
        }

        return birds;
    }

    public async Task AddBirdAsync(Bird bird)
    {
        // Ensure PartitionKey and RowKey are set
        if (string.IsNullOrEmpty(bird.PartitionKey) || string.IsNullOrEmpty(bird.RowKey))
        {
            throw new ArgumentException("PartitionKey and RowKey must be set.");
        }

        try
        {
            await _tableClient.AddEntityAsync(bird);
        }
        catch (RequestFailedException ex)
        {
            // Handle exception as necessary, for example log it or rethrow
            throw new InvalidOperationException("Error adding entity to Table Storage", ex);
        }
    }

    public async Task DeleteBirdAsync(string partitionKey, string rowKey)
    {
        await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task<Bird?> GetBirdAsync(string partitionKey, string rowKey)
    {
        try
        {
            var response = await _tableClient.GetEntityAsync<Bird>(partitionKey, rowKey);
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            // Handle not found
            return null;
        }
    }
}


