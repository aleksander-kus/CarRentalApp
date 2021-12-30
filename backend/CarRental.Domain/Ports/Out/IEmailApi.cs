using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Domain.Ports.Out
{
    public interface IEmailApi
    {
        Task SendEmail(Domain.Dto.Email email, string emailTo, string userTo);
        Task SendEmails(Domain.Dto.Email email, List<(string email, string user)> addresses);
    }
}