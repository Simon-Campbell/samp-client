using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAMP.Client.Services
{
    public interface IServerDiscoveryService
    {
        IList<Server> GetServers();
        IList<Server> GetFavouriteServers();
        IList<Server> GetHostedServers();
    }
}
