﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Models
{
    public class Server : IServer
    {
        public bool IsHosted
        {
            get;
            set;
        }

        public string IpAddress
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

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
                    this.IpAddress == server.IpAddress &&
                    this.Port == server.Port
                );
        }

        public override int GetHashCode()
        {
            return 
                this.IpAddress.GetHashCode() ^ this.Port.GetHashCode();
        }
    }
}