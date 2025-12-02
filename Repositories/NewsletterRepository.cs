using MongoDB.Driver;
using AIOS.Models;

public class NewsletterRepository
{
    private readonly IMongoCollection<Newsletter> _collection;

    public NewsletterRepository(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDbSettings:ConnectionString"]);
        var db = client.GetDatabase(config["MongoDbSettings:DatabaseName"]);

        _collection = db.GetCollection<Newsletter>("Newsletter");
    }

    public async Task<List<Newsletter>> GetAllAsync() =>
        await _collection.Find(_ => true).SortByDescending(x => x.PublishedOn).ToListAsync();

    public async Task<Newsletter?> GetLatestAsync() =>
        await _collection.Find(_ => true).SortByDescending(x => x.PublishedOn).FirstOrDefaultAsync();

    public async Task CreateAsync(Newsletter newsletter) =>
        await _collection.InsertOneAsync(newsletter);
}
