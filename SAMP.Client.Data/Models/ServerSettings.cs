using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Models
{
    public class ServerSettings : ApplicationSettingsBase
    {
        private static ServerSettings defaultInstance = ((ServerSettings)(ApplicationSettingsBase.Synchronized(new ServerSettings())));

        public static ServerSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting]
        public string Username { get; set; }

        [UserScopedSetting]
        public int TimesPlayed { get; set; }

        [UserScopedSetting]
        public DateTime LastPlayed { get; set; }
    }
}
