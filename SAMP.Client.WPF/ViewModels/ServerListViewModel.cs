using Caliburn.Micro;
using SAMP.Client.Data.Models;
using SAMP.Client.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SAMP.Client.WPF.ViewModels
{
    public class ServerListViewModel : PropertyChangedBase
    {
        readonly Func<Task<IEnumerable<Server>>> _updateServerListFunction;
        readonly IServerDetailsService _serverDetailsService;

        IObservableCollection<Server> _servers;

        public string Title { get; set; }

        public int TotalServers
        {
            get
            {
                if (Servers != null)
                    return Servers.Count();

                return 0;
            }
        }

        IObservableCollection<ServerListItemViewModel> _serverListItemViewModels;

        public IEnumerable<ServerListItemViewModel> ServerListItems
        {
            get { return _serverListItemViewModels; }
        }

        public ICommand LoadVisibleServerDetails { get; set; }

        public async void FillDetails(IEnumerable<Server> servers)
        {
            foreach (var server in servers)
            {
                await Task
                    .Run(() => _serverDetailsService.GetDetails(server))
                    .ContinueWith((Task<Server> t) =>
                    {
                        if (t.Result != null)
                        {
                            var result = t.Result;
                            var viewModel = ServerListItems.Single(x => x.Hostname == result.HostName && x.Port == result.Port);

                            viewModel.Name = result.Name;
                        }
                    })
                    .ConfigureAwait(false);
            }
        }

        public IObservableCollection<Server> Servers
        {
            get
            {
                return _servers;
            }
            set
            {
                if (value == null)
                {
                    if (_serverListItemViewModels == null)
                        _serverListItemViewModels = new BindableCollection<ServerListItemViewModel>();

                    _serverListItemViewModels.Clear();
                }
                else
                {
                    _serverListItemViewModels = new BindableCollection<ServerListItemViewModel>(value.Select(s => new ServerListItemViewModel
                    {
                        CountryCode = "NZ",
                        Hostname = s.HostName,
                        Name = s.Name ?? "Unknown",
                        Port = s.Port
                    })); ;
                }

                _servers = value;

                NotifyOfPropertyChange(() => Servers);
                NotifyOfPropertyChange(() => ServerListItems);
                NotifyOfPropertyChange(() => TotalServers);
            }
        }

        public async void Update()
        {
            IsLoading = true;
            Servers = new BindableCollection<Server>(await _updateServerListFunction());
            IsLoading = false;

            FillDetails(Servers.Take(16));
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;

                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public ServerListViewModel(string title, Func<Task<IEnumerable<Server>>> updateServerListFunction, IServerDetailsService serverDetailsService)
        {
            Title = title;

            if (updateServerListFunction != null)
            {
                _updateServerListFunction = updateServerListFunction;
            }
            else
            {
                _updateServerListFunction = () => Task.FromResult((IEnumerable<Server>)new List<Server>());
            }

            _serverDetailsService = serverDetailsService;
            _serverListScrolled = new DelegateCommand((o) => Debug.WriteLine("{0}", Convert.ToString(o)));
        }

        readonly ICommand _serverListScrolled;

        public ICommand ServerListScrolled
        {
            get { 
                return _serverListScrolled; 
            }
        }

        //public void ServerListScrolled()
        //{
        //    Debug.WriteLine("Scrolled!");
        //}

        //public void ServerListScrolled(object src)
        //{
        //    Debug.WriteLine(src.ToString());
        //}
    }
}
