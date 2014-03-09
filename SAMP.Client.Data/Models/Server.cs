using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Models
{
    public class Server
    {
        public string Name { get; set; }

        public bool IsPassworded { get; set; }

        public bool IsHosted { get; set; }

        public string HostName { get;set; }

        public int Port { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var server = obj as Server;
            return this.Equals(server);
        }

        public bool Equals(Server server)
        {
            if (server == null)
            {
                return false;
            }

            return
                (
                    this.HostName == server.HostName &&
                    this.Port == server.Port
                );
        }

        public override int GetHashCode()
        {
            return
                this.HostName.GetHashCode() ^ this.Port.GetHashCode();
        }

        public int CurrentPlayerCount { get; set; }

        public int MaximumPlayerCount { get; set; }

        public string GameMode { get; set; }

        public string MapName { get; set; }
    }
}
