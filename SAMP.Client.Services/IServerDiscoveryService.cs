using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAMP.Client.Services
{
    public interface IServerDiscoveryService
    {
        public IList<IServer> GetServers();
        public IList<IServer> GetFavouriteServers();
        public IList<IServer> GetHostedServers();
    }
}
