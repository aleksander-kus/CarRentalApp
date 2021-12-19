using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class EmailService : INotifyUserAfterCarRent
    {
        private readonly IEmailApi _emailApi;

        public EmailService(IEmailApi emailApi)
        {
            _emailApi = emailApi;
        }

        public async Task NotifyUserAfterCarRent(UserDetails userDetails, CarRentRequest carRentRequest)
        {
            await _emailApi.SendEmail(new Email
            {
                Subject = "Car rent confirmation",
                ToMail = userDetails.Email,
                ToName = $"{userDetails.FirstName} {userDetails.LastName}",
                PlainTextContent = $"Your car was booked from {carRentRequest.RentFrom} to {carRentRequest.RentTo}.",
                HtmlContent = null
            });
        }
    }
}