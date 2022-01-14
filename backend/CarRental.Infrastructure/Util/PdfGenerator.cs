using System.IO;
using System.Threading.Tasks;
using CarRental.Domain.Ports.Out;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;

namespace CarRental.Infrastructure.Util
{
    public class PdfGenerator : IPdfGenerator
    {
        public async Task<Stream> GeneratePdf(string title, string description)
        {
            var document = new PdfDocument();
            document.Info.Title = title;

            var page = document.AddPage();

            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 14, XFontStyle.Regular);
            var tf = new XTextFormatter(gfx);
            var rect = new XRect(40, 70, 520, 100);
            tf.DrawString(description, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            var stream = new MemoryStream();
            document.Save(stream, false);
            return stream;
        }
    }
}