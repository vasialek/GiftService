using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface IConfigurationBll
    {
        MySettings Get();
    }
    public class ConfigurationBll : IConfigurationBll

    {
        private MySettings _settings = null;

        public MySettings Get()
        {
            if (_settings == null)
            {
                _settings = new MySettings();
                _settings.PathToPdfStorage = "c:\\temp\\giftservice\\";
            }
            return _settings;
        }
    }
}
