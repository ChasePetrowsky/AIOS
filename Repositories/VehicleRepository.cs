using AIOS.Models;
using MongoDB.Driver;

namespace AIOS.Repositories
{
    public class VehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;

        public VehicleRepository(IConfiguration config)
        {
            string? connectionString = config["MongoDbSettings:ConnectionString"];
            string? databaseName = config["MongoDbSettings:DatabaseName"];
            string? collectionName = config["MongoDbSettings:VehiclesCollectionName"];

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _vehicles = database.GetCollection<Vehicle>(collectionName);
        }

        public async Task<List<Vehicle>> GetAllAsync() =>
            await _vehicles.Find(_ => true).ToListAsync();

        public async Task<Vehicle?> GetByIdAsync(string id) =>
            await _vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Vehicle vehicle) =>
            await _vehicles.InsertOneAsync(vehicle);

        public async Task UpdateAsync(Vehicle vehicle) =>
            await _vehicles.ReplaceOneAsync(v => v.Id == vehicle.Id, vehicle);

        public async Task DeleteAsync(string id) =>
            await _vehicles.DeleteOneAsync(v => v.Id == id);
    }
}
