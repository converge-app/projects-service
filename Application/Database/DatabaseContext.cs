using Application.Models;
using Application.Models.Entities;
using Application.Utility.Database;
using MongoDB.Driver;

namespace Application.Database {
    public interface IDatabaseContext {
        IMongoCollection<Project> Projects { get; }

        bool IsConnectionOpen ();
    }

    public class DatabaseContext : IDatabaseContext {
        private readonly IMongoDatabase _database;

        public DatabaseContext (IDatabaseSettings settings) {
            var mongoSettings = settings.GetSettings ();
            var client = new MongoClient (mongoSettings);
            _database = client.GetDatabase (settings.DatabaseName);
        }

        public IMongoCollection<Project> Projects => _database.GetCollection<Project> ("Projects");

        public bool IsConnectionOpen () {
            return _database != null;
        }
    }
}