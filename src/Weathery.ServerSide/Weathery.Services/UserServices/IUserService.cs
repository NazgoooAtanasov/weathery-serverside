namespace Weathery.Services.UserServices
{
    using System.Threading.Tasks;
    using Weathery.ViewModels.UserServicesViewModels;

    public interface IUserService
    {
        Task<bool> CreateAsync(string username, string password);

        Task<GetUserViewModel> GetAsync(string username);

    }
}
