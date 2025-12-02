using AIOS.Configuration;
using AIOS.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AIOS.Models;

namespace AIOS.Repositories
{
    public class CustomerRepository
    {
        private readonly IMongoCollection<Customer> _customers;

        public CustomerRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _customers = database.GetCollection<Customer>(settings.Value.CustomersCollectionName);
        }

        // Get all customers
        public async Task<List<Customer>> GetAllAsync()
        {
            return await _customers.Find(_ => true).ToListAsync();
        }

        // Get customer by ID
        public async Task<Customer?> GetByIdAsync(string id)
        {
            return await _customers.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        // Create new customer
        public async Task<Customer> CreateAsync(Customer customer)
        {
            await _customers.InsertOneAsync(customer);
            return customer;
        }

        // Update customer
        public async Task<bool> UpdateAsync(string id, Customer customer)
        {
            var result = await _customers.ReplaceOneAsync(c => c.Id == id, customer);
            return result.ModifiedCount > 0;
        }

        // Delete customer
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _customers.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        // Search customers
        public async Task<List<Customer>> SearchAsync(string searchTerm, string filterType = "all")
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            FilterDefinition<Customer> filter = filterType.ToLower() switch
            {
                "name" => Builders<Customer>.Filter.Regex(c => c.Name, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                "email" => Builders<Customer>.Filter.Regex(c => c.Email, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                "phone" => Builders<Customer>.Filter.Regex(c => c.Phone, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                "vehicle" => Builders<Customer>.Filter.Regex(c => c.Vehicle, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                "vin" => Builders<Customer>.Filter.Regex(c => c.VIN, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                _ => Builders<Customer>.Filter.Or(
                    Builders<Customer>.Filter.Regex(c => c.Name, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Customer>.Filter.Regex(c => c.Email, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Customer>.Filter.Regex(c => c.Phone, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Customer>.Filter.Regex(c => c.Vehicle, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Customer>.Filter.Regex(c => c.VIN, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
                )
            };

            return await _customers.Find(filter).ToListAsync();
        }
    }
}