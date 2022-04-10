
namespace Bookstore.WebApi.ViewModel
{
    public class MessageEmail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public MessageEmail(string to, string subject, string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }
    }
}
