using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Queries.Implementations
{
    public class ServerListQuery : IServerListQuery
    {
        readonly string _versionString = "0.3.6";

        readonly string _hostedActionUrl;
        readonly string _allActionUrl;

        public ServerListQuery()
        {
            _hostedActionUrl    = String.Format("{0}/hosted", _versionString);
            _allActionUrl       = String.Format("{0}/servers", _versionString);
        }

        public List<Models.IServer> All()
        {
            return AllEnumerable().ToList();
        }

        private IEnumerable<Models.IServer> AllEnumerable()
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("http://lists.sa-mp.com") })
            {
                var allServers      = GetAllTask(httpClient);
                var hostedServers   = GetHostedTask(httpClient).Result.ToList();

                foreach (var server in allServers.Result)
                {
                    var hostedServer= hostedServers.FirstOrDefault(x => x.Equals(server));
                    server.IsHosted = hostedServer != null;
                    hostedServers.Remove(hostedServer);
                    yield return server;
                }
            }
        }

        private async Task<IEnumerable<Models.IServer>> GetAllTask(HttpClient httpClient)
        {
            return this.ReadResponse(await httpClient.GetStringAsync(_allActionUrl));
        }

        private async Task<IEnumerable<Models.IServer>> GetHostedTask(HttpClient httpClient)
        {
            return this.ReadResponse(await httpClient.GetStringAsync(_hostedActionUrl));
        }

        public List<Models.IServer> Where(Func<IServer, bool> predicate)
        {
            return 
                AllEnumerable()
                .Where(predicate)
                .ToList();
        }

        private IEnumerable<IServer> ReadResponse(string response)
        {
            if (String.IsNullOrEmpty(response))
                return new List<IServer>();

            return
                response
                .Split('\n')
                .Where(x => !String.IsNullOrEmpty(x))
                .Select((string s) =>
                {
                    var parts = s.Split(':');

                    return
                        (IServer) new Server { IpAddress = parts[0], Port = Int32.Parse(parts[1]) };
                });

        }
    }
}
