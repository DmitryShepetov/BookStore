using System.ComponentModel.DataAnnotations;

namespace Bookstore.WebApi.ViewModel
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string ClientURI { get; set; }
    }
}
