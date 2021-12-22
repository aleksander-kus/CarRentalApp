using System;
using System.Threading.Tasks;

namespace CarRental.Domain.Ports.Out
{
    public interface IEmailApi
    {
        Task SendEmail(Domain.Dto.Email email);
    }
}