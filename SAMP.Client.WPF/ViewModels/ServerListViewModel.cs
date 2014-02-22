using Caliburn.Micro;
using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.WPF.ViewModels
{
    public class ServerListViewModel : PropertyChangedBase
    {
        readonly Func<Task<IEnumerable<Server>>> _updateServerListFunction;

        IEnumerable<Server> _servers;

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

        public IEnumerable<Server> Servers
        {
            get
            {
                return _servers;
            }
            set
            {
                _servers = value;

                NotifyOfPropertyChange(() => Servers);
                NotifyOfPropertyChange(() => TotalServers);
            }
        }

        public async void Update()
        {
            IsLoading = true;
            Servers = await _updateServerListFunction();
            IsLoading = false;
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

        public ServerListViewModel(string title, Func<Task<IEnumerable<Server>>> updateServerListFunction)
        {
            Title = title;

            if (updateServerListFunction != null)
                _updateServerListFunction = updateServerListFunction;
            else
                _updateServerListFunction = () => Task.FromResult((IEnumerable<Server>)new List<Server>());
        }
    }
}
