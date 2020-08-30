namespace Weathery.Services.UserService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Entities;
    using HashService;
    using MongoDB.Driver;
    using TryAtSoftware.OperationResults;
    using Utilities.DatabaseUtilities;
    using ViewModels.UserServicesViewModels;

    public class UserService : IUserService
    {
        private readonly IHashService _hashService;
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IWeatheryDatabaseSettings dbSettings, IHashService hashService)
        {
            this._hashService = hashService;

            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DatabaseName);
            this._usersCollection = database.GetCollection<User>(dbSettings.UsersCollection);
        }

        public async Task<IOperationResult> CreateAsync(string username, string password)
        {
            var operationResult = new OperationResult();

            operationResult.ValidateNotNull(username, $"{nameof(username)} should not have a default value.");
            operationResult.ValidateNotEmpty(username, $"{nameof(username)} should not be empty.");
            operationResult.ValidateNotNull(password, $"{nameof(password)} should not have a default value.");
            operationResult.ValidateNotEmpty(password, $"{nameof(password)} should not be empty.");

            if (operationResult.Success)
            {
                if (await this.IsUsernameFree(username).ConfigureAwait(false))
                {
                    var user = new User
                    {
                        Username = username, Password = this._hashService.Hash(password, username),
                    };

                    await this._usersCollection.InsertOneAsync(user).ConfigureAwait(false);
                    return operationResult;
                }
            }

            return operationResult;
        }

        public async Task<IOperationResult<GetUserViewModel>> GetAsync(string username)
        {
            var operationResult = new OperationResult<GetUserViewModel>();

            operationResult.ValidateNotNull(username, $"{nameof(username)} should not have a default value.");
            operationResult.ValidateNotEmpty(username, $"{nameof(username)} should not be empty.");

            if (operationResult.Success)
            {
                if (await this.IsUsernameFree(username).ConfigureAwait(false) == false)
                {
                    var user = await this.FindUserByUsername(username).ConfigureAwait(false);
                    operationResult.Data =
                        new GetUserViewModel
                        {
                            Id = user.Id, Username = user.Username, SavedCities = user.SavedCities.ToList(),
                        };
                    return operationResult;
                }
            }

            return operationResult;
        }

        public async Task<IOperationResult> SaveCityAsync(string userId, string cityName)
        {
            var operationResult = new OperationResult();

            operationResult.ValidateNotNull(userId, $"{nameof(userId)} should not have a default value.");
            operationResult.ValidateNotEmpty(userId, $"{nameof(userId)} should not be empty.");
            operationResult.ValidateNotNull(cityName, $"{nameof(cityName)} should not have a default value.");
            operationResult.ValidateNotEmpty(cityName, $"{nameof(cityName)} should not be empty.");


            if (operationResult.Success)
            {
                var user = await this.FindUserById(userId).ConfigureAwait(false);
                operationResult.ValidateNotNull(user, $"{nameof(user)} should not be null.");
                if (operationResult.Success == false)
                {
                    return operationResult;
                }

                if (user.SavedCities.Contains(cityName))
                {
                    operationResult.AppendErrorMessage("City already saved");
                    return operationResult;
                }

                user.SavedCities.Add(cityName);
                var update = Builders<User>.Update.Set("SavedCities", user.SavedCities);
                await this._usersCollection.UpdateOneAsync(u => u.Id == user.Id, update).ConfigureAwait(false);
                return operationResult;
            }

            return operationResult;
        }

        public async Task<IOperationResult<ICollection<string>>> GetAllSavedCities(string userId)
        {
            var operationResult = new OperationResult<ICollection<string>>();
            operationResult.ValidateNotNull(userId, $"{nameof(userId)} should not be null");
            operationResult.ValidateNotEmpty(userId, $"{nameof(userId)} should not be empty");

            if (operationResult.Success)
            {
                var user = await this.FindUserById(userId).ConfigureAwait(false);
                operationResult.ValidateNotNull(user, $"{nameof(user)} should not be null.");
                if (operationResult.Success == false)
                {
                    return operationResult;
                }

                operationResult.Data = user.SavedCities.ToList();
                return operationResult;
            }

            return operationResult;
        }

        private async Task<bool> IsUsernameFree(string username)
            => await this._usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync() == null;

        private async Task<User> FindUserById(string id)
            =>
                await this._usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

        private async Task<User> FindUserByUsername(string username)
            =>
                await this._usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync()
                    .ConfigureAwait(false);
    }
}