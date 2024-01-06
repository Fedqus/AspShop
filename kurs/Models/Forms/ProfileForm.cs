using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace kurs.Models.Forms
{
    public class ProfileForm
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [ValidateNever]
        public string Email { get; set; }
    }
}
