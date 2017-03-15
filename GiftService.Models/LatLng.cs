using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models
{

    public enum MapTypes
    {
        YandexMap,
        GoogleMap
    }

    public class LatLng
    {
        public double Lat { get; set; } = Double.MinValue;

        public double Lng { get; set; } = Double.MinValue;

        public bool IsSet
        {
            get { return Lat != Double.MinValue && Lng != Double.MinValue; }
        }

        /// <summary>
        /// Should be set using HelperBll.GetLatLngString()
        /// </summary>
        public string LatLngString { get; set; } = "0.000000,0.000000";
    }
}
