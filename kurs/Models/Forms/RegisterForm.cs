using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace kurs.Models.Forms
{
    public class RegisterForm
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [DisplayName("City")]
        public int CityId { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [ValidateNever]
        public User User
        {
            get => new()
            {
                Firstname = Firstname,
                Lastname = Lastname,
                Address = Address,
                Email = Email,
                UserName = Email,
                CityId = CityId,
            };
        }
    }
}
