using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class EmailService
    {
        private readonly IEmailApi _emailApi;
        private readonly UserService _userService;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly StorageService _storageService;

        public EmailService()
        {
        }
        
        public EmailService(IEmailApi emailApi, UserService userService, IPdfGenerator pdfGenerator, StorageService storageService)
        {
            _emailApi = emailApi;
            _userService = userService;
            _pdfGenerator = pdfGenerator;
            _storageService = storageService;
        }

        public virtual async Task NotifyUserAfterCarRent(UserDetails userDetails, CarRentRequest carRentRequest, CarHistoryEntry entry, string rentId)
        {
            var pdfStream = await _pdfGenerator.GeneratePdf("Car rent confirmation",$"Car: {entry.CarBrand} {entry.CarModel}\n" +
                                            $"Provider: {entry.CarProvider}\n" +
                                            $"Person renting: {userDetails.FirstName} {userDetails.LastName}\n" +
                                            $"Rent date: {carRentRequest.RentFrom}\n" +
                                            $"Return date: {carRentRequest.RentTo}\n");
            var fileName = await _storageService.UploadFileAsync(pdfStream, $"rentConfirm{rentId}.pdf");
            var uri = await _storageService.GetFileSasAsync(fileName, DateTimeOffset.Now.AddHours(1));
            await _emailApi.SendEmail(new Email
            {
                Subject = "Car rent confirmation",
                PlainTextContent = $"Your {entry.CarBrand} {entry.CarModel} was booked from {carRentRequest.RentFrom} to {carRentRequest.RentTo}.\n\n" +
                                   $"Car reservation confirmation (the link will expire after one hour from receiving this email)\n" +
                                   uri,
                HtmlContent = null
            }, userDetails.Email, $"{userDetails.FirstName} {userDetails.LastName}");
        }

        public virtual async Task NotifyUserAfterCarReturn(string userEmail)
        {
            await _emailApi.SendEmail(new Email
            {
                Subject = "Car returned confirmation",
                PlainTextContent = "Your car was successfully returned.",
                HtmlContent = null
            }, userEmail, userEmail);
        }
        
        public virtual async Task NotifyAboutNewCars(List<CarDetails> newCars)
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