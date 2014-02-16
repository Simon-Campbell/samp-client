using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public List<Models.Server> All()
        {
            return AllEnumerable().ToList();
        }

        private IEnumerable<Models.Server> AllEnumerable()
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri("http://lists.sa-mp.com") })
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/3.0 (compatible; SA:MP v0.3z)");

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

        private async Task<IEnumerable<Models.Server>> GetAllTask(HttpClient httpClient)
        {
            return this.ReadResponse(await httpClient.GetStringAsync(_allActionUrl));
        }

        private async Task<IEnumerable<Models.Server>> GetHostedTask(HttpClient httpClient)
        {
            return this.ReadResponse(await httpClient.GetStringAsync(_hostedActionUrl));
        }

        public List<Models.Server> Where(Func<Server, bool> predicate)
        {
            return 
                AllEnumerable()
                .Where(predicate)
                .ToList();
        }

        private IEnumerable<Server> ReadResponse(string response)
        {
            if (String.IsNullOrEmpty(response))
                return new List<Server>();

            return
                response
                .Split('\n')
                .Where(x => !String.IsNullOrEmpty(x))
                .Select((string s) =>
                {
                    var parts = s.Split(':');

                    return
                        new Server { Hostname = parts[0], Port = Int32.Parse(parts[1]) };
                });

        }
    }
}
