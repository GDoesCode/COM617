using COM617.Data;
using MongoDB.Driver;

namespace COM617.Services.Identity
{
    /// <summary>
    /// Handles interactions with the User and UserApplication collections in MongoDb.
    /// </summary>
    public class UserService
    {
        private readonly MongoDbService mongoDbService;

        public UserService(MongoDbService mongoDbService)
        {
            this.mongoDbService = mongoDbService;
        }

        public User? GetUser(string email) => mongoDbService.GetDocumentsByFilter<User>(user => user.Email == email).FirstOrDefault();

        public async Task<bool> CreateUser(User user)
        {
            var dbUser = GetUser(user.Email!);
            if (dbUser is null)
            {
                await mongoDbService.CreateDocument(user);
                return true;
            } 
            else
                return false;
        }

        public UserApplication? GetUserApplication(string email) => mongoDbService.GetDocumentsByFilter<UserApplication>(app => app.User.Email == email).FirstOrDefault();

        public async Task CreateUserApplication(UserApplication userApplication) => await mongoDbService.CreateDocument(userApplication);
    }
}
