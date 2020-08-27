namespace Weathery.Services.UserServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TryAtSoftware.OperationResults;
    using ViewModels.UserServicesViewModels;

    public interface IUserService
    {
        Task<IOperationResult> CreateAsync(string username, string password);

        Task<IOperationResult<GetUserViewModel>> GetAsync(string username);

        Task<IOperationResult> SaveCityAsync(string userId, string cityName);

        Task<IOperationResult<ICollection<string>>> GetAllSavedCities(string userId);
    }
}