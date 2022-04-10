using System.ComponentModel.DataAnnotations;

namespace Bookstore.WebApi.ViewModel
{
    public class TwoFactor
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
