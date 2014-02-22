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
        readonly IServerDetailsService _serverDetailsService;

        IEnumerable<Server> _servers;
        IEnumerable<ServerListViewModel> _serverLists;
        ServerListViewModel _selectedServerList;

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

        public IEnumerable<ServerListViewModel> ServerLists
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

        public string TotalServersStatus
        {
            get
            {
                return String.Format("{0}/{1} shown", SelectedServerList.TotalServers, TotalServers);
            }
        }

        public IEnumerable<Server> Servers
        {
            get { return _servers; }
            private set 
            {
                _servers = value;

                NotifyOfPropertyChange(() => Servers);
                NotifyOfPropertyChange(() => TotalServersStatus);
                NotifyOfPropertyChange(() => CanShowAllServers);
            }
        }

        public Server SelectedServer { get; set; }

        public MainViewModel(
            IServerDiscoveryService serverDiscoveryService, 
            IConfigurationService configurationService,
            IServerDetailsService serverDetailsService)
        {
            _serverDiscoveryService = serverDiscoveryService;
            _configurationService = configurationService;
            _serverDetailsService = serverDetailsService;

            CreateTabs();
        }

        private void CreateTabs()
        {
            var tabs = new List<ServerListViewModel>();

            tabs.Add(new ServerListViewModel("Favourites", null));
            tabs.Add(new ServerListViewModel("All", ShowAllServers));
            tabs.Add(new ServerListViewModel("Hosted", () => Task.FromResult(Servers.Where(s => s.IsHosted))));

            foreach (var tab in tabs)
            {
                tab.PropertyChanged += tab_PropertyChanged;
            }

            ServerLists = tabs;
            SelectedServerList = tabs[0];
        }

        void tab_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalServers") this.NotifyOfPropertyChange(() => TotalServersStatus);
        }
        
        public async Task<IEnumerable<Server>> ShowAllServers()
        {
            return Servers = await Task.Run(() => _serverDiscoveryService.GetServers());
        }

        //var firstServers = Servers.Take(10);
        //var tasks = new List<Task>();

        //foreach (var server in firstServers) 
        //{
        //    var task = Task
        //        .Run(() => _serverDetailsService.GetDetails(server))
        //        .ContinueWith((t) => NotifyOfPropertyChange(() => Servers));

        //    tasks.Add(task);
        //}

        //// Wait until they have all completed!
        //await Task.WhenAll(tasks);

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
