using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Models
{
    public sealed class BrowserSettings : ApplicationSettingsBase
    {
        private static BrowserSettings defaultInstance = ((BrowserSettings) (ApplicationSettingsBase.Synchronized(new BrowserSettings())));

        public static BrowserSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string Username
        {
            get
            {
                return ((string)(this["Username"]));
            }
            set
            {
                this["Username"] = value;
            }
        }

        [UserScopedSetting]
        public List<Server> FavouriteServers
        {
            get
            {
                return (List<Server>)(this["FavouriteServers"]);
            }
            set
            {
                this["FavouriteServers"] = value;
            }
        }

        [UserScopedSetting]
        public List<Server> RecentServers
        {
            get
            {
                return (List<Server>)(this["RecentServers"]);
            }
            set
            {
                this["RecentServers"] = value;
            }
        }
    }

}
