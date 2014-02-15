using SAMP.Client.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Services
{
    public class ServerDiscoveryService : IServerDiscoveryService
    {
        readonly IServerListQuery _serverQuery;
        readonly IConfigurationService _configurationService;

        public ServerDiscoveryService(IServerListQuery serverQuery, IConfigurationService configurationService)
        {
            _serverQuery = serverQuery;
            _configurationService = configurationService;
        }

        public IList<Data.Models.IServer> GetServers()
        {
            return _serverQuery.All();
        }

        public IList<Data.Models.IServer> GetFavouriteServers()
        {
            throw new NotImplementedException();
        }

        public IList<Data.Models.IServer> GetHostedServers()
        {
            _serverQuery.All().Where(x => x.IsHosted);
        }
    }
}
