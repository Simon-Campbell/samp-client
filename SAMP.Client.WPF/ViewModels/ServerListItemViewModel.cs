using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.WPF.ViewModels
{
    public class ServerListItemViewModel : PropertyChangedBase
    {
        public string CountryCode { get; set; }

        public string Name { get; set; }

        public bool NameSpecified 
        {
            get { return String.IsNullOrEmpty(Name); }
        }

        public string Hostname { get; set; }

        public int Port { get; set; }

        public int Ping { get; set; }

        public bool PingSpecified { get { return Ping != 0; } }
    }
}
