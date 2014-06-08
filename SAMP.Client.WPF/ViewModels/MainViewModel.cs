using Caliburn.Micro;
using SAMP.Client.Data.Models;
using SAMP.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SAMP.Client.WPF.ViewModels
{
    public class MainViewModel : PropertyChangedBase, IDeactivate, IHaveDisplayName
    {
        readonly IServerDiscoveryService _serverDiscoveryService;
        readonly IConfigurationService _configurationService;
        readonly IServerDetailsService _serverDetailsService;
        readonly IPingService _pingService;

        IObservableCollection<Server> _servers;
        IObservableCollection<ServerListViewModel> _serverLists;

        ServerListViewModel _selectedServerList;
        DispatcherTimer _dispatcherTimer;

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

        public IObservableCollection<ServerListViewModel> ServerLists
        {
            get { return _serverLists; }
            set 
            { 
                _serverLists = value;

                NotifyOfPropertyChange(() => ServerLists);
            }
        }

        public ServerListViewModel SelectedServerList
        {
            get { return _selectedServerList; }
            set
            {
                _selectedServerList = value;
                _selectedServerList.Update();
            }
        }

        public int TotalServers
        { 
            get 
            {
                if (_servers != null)
                    return _servers.Count();

                return 0;
            }
        }

        public IObservableCollection<Server> Servers
        {
            get { return _servers; }
            private set 
            {
                _servers = value;

                NotifyOfPropertyChange(() => Servers);
                NotifyOfPropertyChange(() => CanShowAllServers);
            }
        }

        public Server SelectedServer { get; set; }

        public MainViewModel(
            IServerDiscoveryService serverDiscoveryService, 
            IConfigurationService configurationService,
            IServerDetailsService serverDetailsService,
            IPingService pingService)
        {
            _serverDiscoveryService = serverDiscoveryService;
            _configurationService = configurationService;
            _serverDetailsService = serverDetailsService;
            _pingService = pingService;

            SetWindowTitle();

            CreatePingTimer();
            CreateTabs();
        }

        private void CreatePingTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            _dispatcherTimer.Tick += (sender, args) => 
            {
                if (_selectedServerList.ServerListItems == null) 
                    return;

                Task.Run(() => { 
                    foreach (var server in _selectedServerList.ServerListItems.Take(16))
                    {
                        server.Ping = _pingService.GetPing(server.Hostname);
                    }
                });
            };
            _dispatcherTimer.Start();
        }

        private void SetWindowTitle()
        {
            DisplayName = "San Andreas Multiplayer";
        }

        private void CreateTabs()
        {
            var tabs = new BindableCollection<ServerListViewModel>();

            tabs.Add(new ServerListViewModel("Favourites", GetFavouritesAsync, _serverDetailsService));
            tabs.Add(new ServerListViewModel("All", GetAllServersAsync, _serverDetailsService));
            tabs.Add(new ServerListViewModel("Hosted", GetHostedServersAsync, _serverDetailsService));

            ServerLists = tabs;
            SelectedServerList = tabs[0];
        }

        private void SaveFavourites(IEnumerable<ServerListItemViewModel> previous, IEnumerable<ServerListItemViewModel> updated)
        {
            var favourites = previous != null? previous.Where(serverViewModel => serverViewModel.IsFavourited).ToList() : new List<ServerListItemViewModel>();

            foreach (var serverViewModel in updated)
            {
                serverViewModel.IsFavourited = favourites.Any(favourite => favourite.Hostname == serverViewModel.Hostname && favourite.Port == serverViewModel.Port);
            }

            _configurationService.BrowserSettings.FavouriteServers = favourites.Select(favourite => new Server { HostName = favourite.Hostname, Port = favourite.Port }).ToList();
        }

        public async Task<IEnumerable<Server>> GetFavouritesAsync()
        {
            return await Task.Run(() => {
                var favourites = _configurationService.BrowserSettings.FavouriteServers;

                if (favourites == null)
                {
                    favourites = new List<Server>();
                }

                if (!favourites.Any())
                {
                    favourites.Add(new Server {
                        HostName = "littlewhiteys.co.uk",
                        Port     = 7778
                    });
                }

                return (_configurationService.BrowserSettings.FavouriteServers = favourites);
            });
        }

        public async Task<IEnumerable<Server>> GetHostedServersAsync()
        {
            if (Servers == null || !Servers.Any())
            {
                await GetAllServersAsync();
            }

            return Servers.Where(s => s.IsHosted);
        }

        public async Task<IEnumerable<Server>> GetAllServersAsync()
        {
            if (Servers == null || !Servers.Any())
            {
                Servers = new BindableCollection<Server>(await Task.Run(() => _serverDiscoveryService.GetServers()));
            }

            return Servers;
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

            var browserSettings = _configurationService.BrowserSettings;
            browserSettings.Save();
            
            if (Deactivated != null) 
                Deactivated(this, new DeactivationEventArgs { WasClosed = close });
        }

        public string DisplayName { get; set; }

        public bool CanRefreshSelectedList
        {
            get { return true; }
        }

        public void RefreshSelectedList()
        {
            SelectedServerList.Update();
        }
    }
}
