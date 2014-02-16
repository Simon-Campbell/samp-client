using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Queries
{
    public interface IServerDetailsQuery
    {
        /// <summary>
        /// Get the details of a server specified. The requirements for this to work is that the
        /// server has both a valid hostname and port.
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        Server GetDetails(Server server);
    }
}
