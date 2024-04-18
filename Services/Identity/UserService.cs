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

        public List<User> GetUsers() => mongoDbService.GetQueryableCollection<User>().ToList();

        public User? GetUser(string email) => mongoDbService.GetDocumentsByFilter<User>(user => user.Email.ToLower().Equals(email.ToLower())).FirstOrDefault();

        public event EventHandler<User>? OnUserCreated;
        public event EventHandler<User>? OnUserUpdated;
        public event EventHandler<User>? OnUserDeleted;

        public async Task<bool> CreateUser(User user)
        {
            var dbUser = GetUser(user.Email!);
            if (dbUser is null)
            {
                await mongoDbService.CreateDocument(user);
                OnUserCreated?.Invoke(this, user);
                return true;
            } 
            else
                return false;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var dbUser = GetUser(user.Email!);
            if (dbUser is null)
                return false;
            else
            {
                await mongoDbService.ReplaceDocument(user.Id, user);
                OnUserUpdated?.Invoke(this, user);
                return true;
            }
        }

        public async Task<bool> DeleteUser(User user)
        {
            var dbUser = GetUser(user.Email!);
            if (dbUser is null)
                return false;
            else
            {
                await mongoDbService.DeleteDocument<User>(user.Id);
                OnUserDeleted?.Invoke(this, user);
                return true;
            }
        }

        public UserApplication? GetUserApplication(string email) => mongoDbService.GetDocumentsByFilter<UserApplication>(app => app.User.Email == email).FirstOrDefault();

        public async Task CreateUserApplication(UserApplication userApplication) => await mongoDbService.CreateDocument(userApplication);
    }
}
