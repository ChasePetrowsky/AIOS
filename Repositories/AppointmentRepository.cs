using MongoDB.Driver;
using Microsoft.Extensions.Options;
using AIOS.Configuration;
using AIOS.Models;

namespace AIOS.Repositories
{
    public class AppointmentRepository
    {
        private readonly IMongoCollection<Appointment> _appointments;

        public AppointmentRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _appointments = database.GetCollection<Appointment>("Appointments");
        }

        // Get all appointments
        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _appointments.Find(_ => true).ToListAsync();
        }

        // Get appointment by ID
        public async Task<Appointment?> GetByIdAsync(string id)
        {
            return await _appointments.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        // Get appointments by customer ID
        public async Task<List<Appointment>> GetByCustomerIdAsync(string customerId)
        {
            return await _appointments.Find(a => a.CustomerId == customerId).ToListAsync();
        }

        // Get appointments in date range
        public async Task<List<Appointment>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var filter = Builders<Appointment>.Filter.And(
                Builders<Appointment>.Filter.Gte(a => a.Start, start),
                Builders<Appointment>.Filter.Lte(a => a.End, end)
            );
            return await _appointments.Find(filter).ToListAsync();
        }

        // Create new appointment
        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            await _appointments.InsertOneAsync(appointment);
            return appointment;
        }

        // Update appointment
        public async Task<bool> UpdateAsync(string id, Appointment appointment)
        {
            var result = await _appointments.ReplaceOneAsync(a => a.Id == id, appointment);
            return result.ModifiedCount > 0;
        }

        // Delete appointment
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _appointments.DeleteOneAsync(a => a.Id == id);
            return result.DeletedCount > 0;
        }
    }
}