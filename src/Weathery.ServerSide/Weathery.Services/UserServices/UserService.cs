namespace Weathery.Services.UserServices
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
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
                // Extract to method.
                var user = await this.FindUserByUsername(username).ConfigureAwait(false);
                return new GetUserViewModel { Id = user.Id, Username = user.Username, SavedCities = user.SavedCities.ToList() };
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> SaveCityAsync(string id, string cityName)
        {
            // Extract to method.
            var user = await this.FindUserById(id).ConfigureAwait(false);
            if (user == null)
            {
                return false;
            }
            if (user.SavedCities.Contains(cityName) == true)
            {
                return false;
            }
            user.SavedCities.Add(cityName);
            var update = Builders<User>.Update.Set("SavedCities", user.SavedCities);
            await this.usersCollection.UpdateOneAsync(u => u.Id == user.Id, update).ConfigureAwait(false);
            return true;
        }

        public async Task<ICollection<string>> GetAllSavedCities(string id)
        {
            var user = await this.FindUserById(id).ConfigureAwait(false);
            return user.SavedCities.ToList();
        }

        private async Task<bool> IsUsernameFree(string username)
          => await this.usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync() == null ? true : false;

        private async Task<User> FindUserById(string id)
            =>
                await this.usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

        private async Task<User> FindUserByUsername(string username)
            =>
                await this.usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync().ConfigureAwait(false);
    }
}
