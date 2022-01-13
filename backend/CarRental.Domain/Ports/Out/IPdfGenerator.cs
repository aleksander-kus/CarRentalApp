using System.IO;
using System.Threading.Tasks;

namespace CarRental.Domain.Ports.Out
{
    public interface IPdfGenerator
    {
        Task<Stream> GeneratePdf(string title, string description);
    }
}