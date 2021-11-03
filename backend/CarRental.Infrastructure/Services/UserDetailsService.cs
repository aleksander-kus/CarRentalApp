using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports;
using Microsoft.Graph;

namespace CarRental.Infrastructure.Services
{
    public class UserDetailsService: IGetUserDetailsUseCase
    {
        private readonly string _extensionPrefix = "extension_2d4ef52895db4c60b4c09f5bd807fd63_";
        private readonly IAuthenticationProvider _msGraphAuthenticationProvider;

        public UserDetailsService(IAuthenticationProvider authenticationProvider)
        {
            _msGraphAuthenticationProvider = authenticationProvider;
        }

        public async Task<UserDetails> GetUserDetails(string userId)
        {
            var graphClient = new GraphServiceClient(_msGraphAuthenticationProvider);

            var user = await graphClient.Users[userId].Request()
                .Select($"{_extensionPrefix}Age,{_extensionPrefix}YearsOfHavingDrivingLicense," +
                        $"City,Country,PostalCode,StreetAddress,GivenName,Surname,otherMails")
                .GetAsync();

            var age = ((JsonElement)user.AdditionalData[$"{_extensionPrefix}Age"]).GetInt32();
            var yearsD = ((JsonElement)user.AdditionalData[$"{_extensionPrefix}YearsOfHavingDrivingLicense"]).GetInt32();

            var userDetails = new UserDetails()
            {
                Address = user.StreetAddress,
                Age = age,
                City = user.City,
                Email = user.OtherMails.ToList().ToList()[0],
                FirstName = user.GivenName,
                LastName = user.Surname,
                PostalCode = user.PostalCode,
                UserId = userId,
                YearsHavingDrivingLicense = yearsD
            };

            return userDetails;
        }
    }
}