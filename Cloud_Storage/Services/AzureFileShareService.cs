using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Cloud_Storage.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AzureFileShareService
{
    private readonly string _connectionString;
    private readonly string _fileShareName;

    public AzureFileShareService(string connectionString, string fileShareName)
    {
        _connectionString = connectionString;
        _fileShareName = fileShareName;
    }

    // List files in a directory and return a list of FileModel objects
    public async Task<List<FileModel>> ListFilesAsync(string directoryName)
    {
        var fileModels = new List<FileModel>();

        try
        {
            var shareClient = new ShareClient(_connectionString, _fileShareName);
            var directoryClient = shareClient.GetDirectoryClient(directoryName);

            await foreach (ShareFileItem item in directoryClient.GetFilesAndDirectoriesAsync())
            {
                if (!item.IsDirectory)
                {
                    var fileClient = directoryClient.GetFileClient(item.Name);
                    var properties = await fileClient.GetPropertiesAsync();

                    fileModels.Add(new FileModel
                    {
                        Name = item.Name,
                        Size = properties.Value.ContentLength,
                        LastModified = properties.Value.LastModified
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error listing files: {ex.Message}");
            throw;
        }

        return fileModels;
    }
}
