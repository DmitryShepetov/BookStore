using Microsoft.AspNetCore.Identity;
namespace Bookstore.WebApi
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
