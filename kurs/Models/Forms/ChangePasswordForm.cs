using System.ComponentModel.DataAnnotations;

namespace kurs.Models.Forms
{
    public class ChangePasswordForm
    {
        public string CurrentPassword { get; set; }
        [MinLength(6)]
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set;}
    }
}
