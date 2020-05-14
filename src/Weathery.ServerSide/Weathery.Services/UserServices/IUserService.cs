namespace Weathery.Services.UserServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Weathery.ViewModels.UserServicesViewModels;

    public interface IUserService
    {
        Task<bool> CreateAsync(string username, string password);

        Task<GetUserViewModel> GetAsync(string username);

        Task<bool> SaveCityAsync(string id, string cityName);

        Task<ICollection<string>> GetAllSavedCities(string id);
    }
}
