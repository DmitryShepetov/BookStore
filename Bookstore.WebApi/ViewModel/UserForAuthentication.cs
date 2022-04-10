using System.ComponentModel.DataAnnotations;


namespace Bookstore.WebApi.ViewModel
{
    public class UserForAuthentication
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
