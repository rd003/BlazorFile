using BlazorFile.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BlazorFile.Repositories;

public interface IImageUploadRepository
{
    Task UploadImageToDb(ImageFile image);
    Task<IEnumerable<ImageFile>> GetImages();
}
public class ImageUploadRepository: IImageUploadRepository
{
    private readonly IConfiguration _config;
    const string CONN_KEY = "default";
    public ImageUploadRepository(IConfiguration config)
    {
        _config = config;
    }
    public async Task UploadImageToDb(ImageFile image)
    {
        var connectionString = _config.GetConnectionString(CONN_KEY);
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "insert into ImageFile (ImageName) values (@ImageName)";
        await connection.ExecuteAsync(sql, new { ImageName = image.ImageName });
    }

    public async Task<IEnumerable<ImageFile>> GetImages()
    {
        var connectionString = _config.GetConnectionString(CONN_KEY);
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "select * from ImageFile";
        var images = await connection.QueryAsync<ImageFile>(sql);
        return images;
    }
}
