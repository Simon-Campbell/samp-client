using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAMP.Client.Data.Models
{
    public interface IServer
    {
        bool IsHosted { get; set; }
        string IpAddress { get; set; }
        int Port { get; set; }
    }
}
