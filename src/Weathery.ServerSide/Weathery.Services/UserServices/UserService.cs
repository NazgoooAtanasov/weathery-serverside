namespace Weathery.Services.UserServices
{
    using MongoDB.Driver;
    using System.Threading.Tasks;
    using Weathery.API.Utilities;
    using Weathery.Data.Entities;
    using Weathery.Services.HashService;
    using Weathery.ViewModels.UserServicesViewModels;

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> usersCollection;
        private readonly IHashService hashService;

        public UserService(IWeatheryDatabaseSettings dbSettings, IHashService hashService)
        {
            this.hashService = hashService;

            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DatabaseName);
            usersCollection = database.GetCollection<User>(dbSettings.UsersCollection);
        }

        public async Task<bool> CreateAsync(string username, string password)
        {
            if (await this.IsUsernameFree(username).ConfigureAwait(false))
            {
                var user = new User
                {
                    Username = username,
                    Password = this.hashService.Hash(password, username)
                };

                await usersCollection.InsertOneAsync(user).ConfigureAwait(false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<GetUserViewModel> GetAsync(string username)
        {
            if (await this.IsUsernameFree(username).ConfigureAwait(false) == false)
            {
                var user = await this.usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();
                return new GetUserViewModel { Id = user.Id, Username = user.Username };
            }
            else
            {
                return null;
            }
        }

        private async Task<bool> IsUsernameFree(string username)
          =>  await this.usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync() == null ? true : false;

    }
}
