using COM617.Data;

namespace COM617.Services.Identity
{
    /// <summary>
    /// Handles interactions with the User collection in MongoDb.
    /// </summary>
    public class UserService
    {
        private readonly MongoDbService mongoDbService;

        public UserService(MongoDbService mongoDbService)
        {
            this.mongoDbService = mongoDbService;
        }

        public User? GetUser(string email) => mongoDbService.GetDocumentsByFilter<User>(user => user.Email == email).FirstOrDefault();

        public async Task CreateUser(User user) => await mongoDbService.CreateDocument(user);

        // Additional methods as needed
    }
}
