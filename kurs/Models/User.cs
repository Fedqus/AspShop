using Microsoft.AspNetCore.Identity;

namespace kurs.Models
{
    public class User : IdentityUser<int>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public bool IsAdmin { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public string Fullname()
        {
            return $"{Firstname} {Lastname}";
        }

    }
}
