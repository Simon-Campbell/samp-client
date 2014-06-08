using Caliburn.Micro;
using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SAMP.Client.WPF.ViewModels
{
    public class ServerListItemViewModel : PropertyChangedBase
    {
        BitmapImage _countryImage = null;
        string _countryImageSource = "pack://application:,,,/Resources/Flags/New-Zealand.png";
        
        int _ping;
        string _name;

        public ServerListItemViewModel()
        {
            SetCountryImage();
        }

        public ServerListItemViewModel(Server model)
        {
            Name     = model.Name;
            Hostname = model.HostName;
            Port     = model.Port;
           
            SetCountryImage();
        }

        public void SetCountryImage()
        {
            CountryImage = new BitmapImage(new Uri(_countryImageSource));
        }

        public string CountryCode { get; set; }

        public BitmapImage CountryImage
        {
            get { return _countryImage; }
            set { _countryImage = value; }
        }

        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;

                NotifyOfPropertyChange(() => Name);
            }
        }

        public bool NameSpecified 
        {
            get { return String.IsNullOrEmpty(Name); }
        }

        public string Hostname { get; set; }

        public int Port { get; set; }
        
        public int Ping
        {
            get
            {
                return _ping;
            }
            set
            {
                _ping = value;

                NotifyOfPropertyChange(() => Ping);
            }
        }

        public bool PingSpecified { get { return Ping != 0; } }

        public bool IsFavourited { get; set; }

        public bool CanToggleFavourite
        {
            get { return true; }
        }

        public void ToggleFavourite()
        {
            IsFavourited = !IsFavourited;

            NotifyOfPropertyChange(() => FavouriteActionSymbol);
        }

        public string FavouriteActionSymbol
        {
            get
            {
                if (IsFavourited)
                {
                    return "\uE249";
                }
                else
                {
                    return "\uE24A";
                }
            }
        }
    }
}
