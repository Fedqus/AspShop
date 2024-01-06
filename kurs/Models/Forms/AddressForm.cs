using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace kurs.Models.Forms
{
    public class AddressForm
    {
        [DisplayName("City")]
        public int CityId { get; set; }
        public string Address { get; set; }
    }
}
