using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Reflection;

namespace COM617.Services
{
    /// <summary>
    /// Facilitates management of MongoDB databases.
    /// </summary>
    public interface IMongoDbService
    {
        /// <summary>
        /// Creates a MongoDB document of a given C# type.
        /// </summary>
        /// <typeparam name="T">The type of the documents</typeparam>
        /// <param name="document">The document.</param>
        Task CreateDocument<T>(T document);

        /// <summary>
        /// Creates multiple MongoDB documents of a given C# type.
        /// </summary>
        /// <typeparam name="T">The type of the documents</typeparam>
        /// <param name="documents">The documents.</param>
        Task CreateDocuments<T>(T[] documents);

        /// <summary>
        /// Replaces a given document.
        /// </summary>
        /// <typeparam name="T">The type of the document.</typeparam>
        /// <param name="id">The Id of the document to replace.</param>
        /// <param name="replacement">The document to replace it with.</param>
        Task<ReplaceOneResult> ReplaceDocument<T>(Guid id, T replacement);

        /// <summary>
        /// Deletes a given document
        /// </summary>
        /// <typeparam name="T">The type of the document.</typeparam>
        /// <param name="id">The Id of the document to delete.</param>
        Task<DeleteResult> DeleteDocument<T>(Guid id);

        /// <summary>
        /// Gets the MongoDB collection of a given C# Type.
        /// </summary>
        /// <typeparam name="T">The type of the documents</typeparam>
        IMongoCollection<T> GetCollection<T>();

        /// <summary>
        /// Gets a queryable MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the documents</typeparam>
        IMongoQueryable<T> GetQueryableCollection<T>();

        /// <summary>
        /// Gets a queryable MongoDB collection as an Array.
        /// </summary>
        /// <typeparam name="T">The type of the documents.</typeparam>
        T[] GetCollectionAsArray<T>();

        /// <summary>
        /// Gets the documents.
        /// </summary>
        /// <typeparam name="T">The type of the documents</typeparam>
        /// <param name="filter">The filter.</param>
        IEnumerable<T> GetDocumentsByFilter<T>(Func<T, bool> filter);
    }

    /// <summary>
    /// Concrete implementation of <see cref="IMongoDbService"/>
    /// </summary>
    internal class MongoDbService : IMongoDbService
    {
        private readonly MongoClient mongoClient;
        private readonly List<MongoTypeMap> collectionTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration (appsettings.json).</param>
        public MongoDbService(IConfiguration configuration)
        {
            mongoClient = new MongoClient(configuration.GetConnectionString("MongoDB"));
            collectionTypes = new();

            RegisterTypes();
        }

        /// <summary>
        /// Returns the <see cref="MongoTypeMap"/> for a given type.
        /// </summary>
        private MongoTypeMap Get<T>() => collectionTypes.Find(x => x.Type == typeof(T))!;

        /// <summary>
        /// Registers all classes with a <see cref="MongoTypeMapAttribute"/> for saving in MongoDB.
        /// </summary>
        private void RegisterTypes()
        {
            var dataClasses = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<MongoTypeMapAttribute>() != null);

            foreach (var dataClass in dataClasses)
            {
                var attribute = dataClass.GetCustomAttribute<MongoTypeMapAttribute>()!;
                attribute.MongoTypeMap.Type = dataClass;
                collectionTypes.Add(attribute.MongoTypeMap);
            }
        }

        /// <inheritdoc/>
        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionType = Get<T>();
            return mongoClient.GetDatabase(collectionType.DatabaseName).GetCollection<T>(collectionType.CollectionName);
        }

        /// <inheritdoc/>
        public IMongoQueryable<T> GetQueryableCollection<T>() => GetCollection<T>().AsQueryable();

        /// <inheritdoc/>
        public T[] GetCollectionAsArray<T>() => GetQueryableCollection<T>().ToArray();

        /// <inheritdoc/>
        public async Task CreateDocument<T>(T document) => await GetCollection<T>().InsertOneAsync(document);

        /// <inheritdoc/>
        public async Task CreateDocuments<T>(T[] documents) => await GetCollection<T>().InsertManyAsync(documents);

        /// <inheritdoc/>
        public async Task<DeleteResult> DeleteDocument<T>(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await GetCollection<T>().DeleteOneAsync(filter);
        }

        /// <inheritdoc/>
        public IEnumerable<T> GetDocumentsByFilter<T>(Func<T, bool> filter) => GetQueryableCollection<T>().Where(filter);

        /// <inheritdoc/>
        public async Task<ReplaceOneResult> ReplaceDocument<T>(Guid id, T replacement)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await GetCollection<T>().ReplaceOneAsync(filter, replacement);
        }
    }

    /// <summary>
    /// Stores information about a MongoDB collection, and its relevant C# type.
    /// </summary>
    public class MongoTypeMap
    {
        /// <summary>
        /// The name of the MongoDB database.
        /// </summary>
        public string DatabaseName { get; }

        /// <summary>
        /// The name of the MongoDB collection.
        /// </summary>
        public string CollectionName { get; }

        /// <summary>
        /// The C# type to be stored.
        /// </summary>
        public Type? Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoTypeMap"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the MongoDB database.</param>
        /// <param name="collectionName">Name of the mongoDB collection.</param>
        /// <param name="type">The type that this MongoDB collection stores.</param>
        public MongoTypeMap(string databaseName, string collectionName, Type? type = null)
        {
            DatabaseName = databaseName;
            CollectionName = collectionName;
            Type ??= type;
        }
    }

    /// <summary>
    /// Registers a class with the <see cref="MongoDbService"/>
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoTypeMapAttribute : Attribute
    {
        /// <summary>
        /// Gets the mongo type mapping.
        /// </summary>
        public MongoTypeMap MongoTypeMap { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoTypeMapAttribute"/> class.
        /// </summary>
        /// <param name="databaseName">The name of the MongoDB collection that stores this type.</param>
        /// <param name="collectionName">The name of the MongoDB collection that stores this type.</param>
        /// <param name="type">The C# type that this mapping represents.</param>
        public MongoTypeMapAttribute(string databaseName, string collectionName)
        {
            MongoTypeMap = new(databaseName, collectionName);
        }
    }
}
