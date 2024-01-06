using System.ComponentModel.DataAnnotations;

namespace kurs.Models.Forms
{
    public class LoginForm
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
