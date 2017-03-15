using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Products
{
    /// <summary>
    /// Place where service is
    /// </summary>
    public class ProductServiceLocation
    {
        public int Id { get; set; }
        public int PosId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string EmailReservation { get; set; }
        public string PhoneReservation { get; set; }

        /// <summary>
        /// Should be set as default POS (on checkout)
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// String to get JSON coordinates
        /// </summary>
        public string LatLng { get; set; }

        public LatLng LatLngCoordinates { get; set; }

        public string NameAddress
        {
            get
            {
                string s = String.IsNullOrEmpty(this.Name.Trim()) ? "" : String.Concat(Name.Trim(), ". ");

                s = String.Concat(s, Address);

                s = String.Concat(s, ", ", City);

                return s;
            }
        }

        public ProductServiceLocation()
        {
            LatLngCoordinates = new LatLng();
        }
    }
}
