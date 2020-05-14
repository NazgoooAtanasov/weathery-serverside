using System.Collections.Generic;

namespace Weathery.ViewModels.UserServicesViewModels
{
    public class GetUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public IList<string> SavedCities { get; set; }
    }
}
