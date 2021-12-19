namespace CarRental.Domain.Dto
{
    public class Email
    {
        public string Subject { get; set; }
        public string ToMail { get; set; }
        public string ToName { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
    }
}