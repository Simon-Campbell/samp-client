using Caliburn.Micro;
using SAMP.Client.Data.Models;
using SAMP.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SAMP.Client.WPF.ViewModels
{
    public class MainViewModel : PropertyChangedBase, IDeactivate
    {
        readonly IServerDiscoveryService _serverDiscoveryService;
        readonly IConfigurationService _configurationService;

        IEnumerable<Server> _servers;

        public string Username
        {
            get
            {
                return _configurationService.BrowserSettings.Username;
            }
            set
            {
                _configurationService.BrowserSettings.Username = value;

                NotifyOfPropertyChange(() => Username);
            }
        }

        public IEnumerable<Server> Servers
        {
            get
            {
                return _servers;
            }
            private set 
            {
                _servers = value;

                NotifyOfPropertyChange(() => Servers);
                NotifyOfPropertyChange(() => CanShowAllServers);
            }
        }

        public Server SelectedServer { get; set; }

        public MainViewModel(IServerDiscoveryService serverDiscoveryService, IConfigurationService configurationService)
        {
            _serverDiscoveryService = serverDiscoveryService;
            _configurationService = configurationService;
        }

        public async void ShowAllServers()
        {
            Servers = await Task.Run(() => _serverDiscoveryService.GetServers());
        }

        public bool CanShowAllServers
        {
            get { return true; }
        }

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;
        public event EventHandler<DeactivationEventArgs> Deactivated;

        public void Deactivate(bool close)
        {
            if (AttemptingDeactivation != null) 
                AttemptingDeactivation(this, new DeactivationEventArgs());

            _configurationService.BrowserSettings.Save();
            
            if (Deactivated != null) 
                Deactivated(this, new DeactivationEventArgs { WasClosed = close });
        }
    }
}
