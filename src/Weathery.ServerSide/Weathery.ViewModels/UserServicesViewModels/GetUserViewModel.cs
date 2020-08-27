namespace Weathery.ViewModels.UserServicesViewModels
{
    using System.Collections.Generic;

    public class GetUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public IList<string> SavedCities { get; set; }
    }
}