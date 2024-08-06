using Azure;
using Azure.Data.Tables;
using Cloud_Storage.Models;
using System.Threading.Tasks;

public class TableStorageService
{
    private readonly TableClient _tableClient;
    private readonly TableClient _birderTableClient;
    private readonly TableClient _sightingTableClient;

    public TableStorageService(string connectionString)
    {
        _tableClient = new TableClient(connectionString, "Birds");
        _birderTableClient = new TableClient(connectionString, "Birders");
        _sightingTableClient = new TableClient(connectionString, "Sighting");
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
    public async Task<List<Birder>> GetAllBirdersAsync()
    {
        var birders = new List<Birder>();

        await foreach (var birder in _birderTableClient.QueryAsync<Birder>())
        {
            birders.Add(birder);
        }

        return birders;
    }
    public async Task AddBirderAsync(Birder birder)
    {
        if (string.IsNullOrEmpty(birder.PartitionKey) || string.IsNullOrEmpty(birder.RowKey))
        {
            throw new ArgumentException("PartitionKey and RowKey must be set.");
        }

        try
        {
            await _birderTableClient.AddEntityAsync(birder);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException("Error adding entity to Table Storage", ex);
        }
    }

    public async Task DeleteBirderAsync(string partitionKey, string rowKey)
    {
        await _birderTableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task<Birder?> GetBirderAsync(string partitionKey, string rowKey)
    {
        try
        {
            var response = await _birderTableClient.GetEntityAsync<Birder>(partitionKey, rowKey);
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public async Task AddSightingAsync(Sighting sighting)
    {
        if (string.IsNullOrEmpty(sighting.PartitionKey) || string.IsNullOrEmpty(sighting.RowKey))
        {
            throw new ArgumentException("PartitionKey and RowKey must be set.");
        }

        try
        {
            await _sightingTableClient.AddEntityAsync(sighting);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException("Error adding sighting to Table Storage", ex);
        }
    }

    
    public async Task<List<Sighting>> GetAllSightingsAsync()
    {
        var sightings = new List<Sighting>();

        await foreach (var sighting in _sightingTableClient.QueryAsync<Sighting>())
        {
            sightings.Add(sighting);
        }

        return sightings;
    }
}


