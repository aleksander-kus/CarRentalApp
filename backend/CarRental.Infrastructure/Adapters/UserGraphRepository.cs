using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.Out;
using Microsoft.Graph;

namespace CarRental.Infrastructure.Adapters
{
    public class UserGraphRepository : IUserRepository
    {
        private readonly string _extensionPrefix = "extension_2d4ef52895db4c60b4c09f5bd807fd63_";
        private readonly GraphServiceClient _graphServiceClient;

        public UserGraphRepository(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<UserDetails> GetUserDetailsAsync(string userId)
        {
            var user = await _graphServiceClient.Users[userId].Request()
                .Select($"{_extensionPrefix}Age,{_extensionPrefix}YearsOfHavingDrivingLicense," +
                        $"City,Mail,Country,PostalCode,StreetAddress,GivenName,Surname,otherMails")
                .GetAsync();

            var age = ((JsonElement)user.AdditionalData[$"{_extensionPrefix}Age"]).GetInt32();
            var yearsD = ((JsonElement)user.AdditionalData[$"{_extensionPrefix}YearsOfHavingDrivingLicense"]).GetInt32();

            var mail = user.OtherMails.Any() ? user.OtherMails.First() : user.Mail;
            
            var userDetails = new UserDetails()
            {
                Address = user.StreetAddress,
                Age = age,
                City = user.City,
                Email = mail,
                FirstName = user.GivenName,
                LastName = user.Surname,
                PostalCode = user.PostalCode,
                UserId = userId,
                YearsHavingDrivingLicense = yearsD
            };

            return userDetails;
        }

        public async Task<List<string>> GetAllEmailsAsync()
        {
            var emails = new LinkedList<string>();
            
            var users = await _graphServiceClient.Users.Request()
                .Select("Mail,otherMails")
                .GetAsync();
            do
            {
                foreach (var user in users)
                {
                    var mail = user.OtherMails.Any() ? user.OtherMails.First() : user.Mail;
                    
                    if (mail != null)
                        emails.AddLast(mail);
                }
            }
            while (users.NextPageRequest != null && (users = await users.NextPageRequest.GetAsync()).Count > 0);

            return emails.ToList();
        }
    }
}