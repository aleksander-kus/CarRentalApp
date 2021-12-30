using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class EmailService
    {
        private readonly IEmailApi _emailApi;
        private readonly UserService _userService;

        public EmailService(IEmailApi emailApi, UserService userService)
        {
            _emailApi = emailApi;
            _userService = userService;
        }

        public async Task NotifyUserAfterCarRent(UserDetails userDetails, CarRentRequest carRentRequest)
        {
            await _emailApi.SendEmail(new Email
            {
                Subject = "Car rent confirmation",
                PlainTextContent = $"Your car was booked from {carRentRequest.RentFrom} to {carRentRequest.RentTo}.",
                HtmlContent = null
            }, userDetails.Email, $"{userDetails.FirstName} {userDetails.LastName}");
        }

        public async Task NotifyAboutNewCars(List<CarDetails> newCars)
        {
            var bodyHtml = GenerateNewCarsEmailBodyHtml(newCars);

            var emails = await _userService.GetAllEmails();

            var emailAddresses = emails.Select(e => (e, e)).ToList();
            
            await _emailApi.SendEmails(new Email
            {
                Subject = "New cars available",
                PlainTextContent = null,
                HtmlContent = bodyHtml
            }, emailAddresses);
        }

        private static string GenerateNewCarsEmailBodyHtml(List<CarDetails> cars)
        {
            var carsListHtml = string.Join("", cars
                .Select(c => $"<li>{c.Brand} {c.Model} ({c.ProductionYear}) from {c.ProviderCompany}</li>"));

            return $"<h1>New available cars:</h1><ul>{carsListHtml}</ul>";
        }
    }
}