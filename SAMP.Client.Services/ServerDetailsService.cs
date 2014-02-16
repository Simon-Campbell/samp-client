using SAMP.Client.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Services
{
    public class ServerDetailsService : IServerDetailsService
    {
        readonly IServerDetailsQuery _serverDetailsQuery;

        public ServerDetailsService(IServerDetailsQuery serverDetailsQuery)
        {
            _serverDetailsQuery = serverDetailsQuery;
        }

        public Data.Models.Server GetDetails(Data.Models.Server server)
        {
            return _serverDetailsQuery.GetDetails(server);
        }
    }
}
