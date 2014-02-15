using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Models
{
    public interface IBrowserSettings
    {
        string DefaultUsername { get; set; }
        IList<IServer> FavouriteServers { get; set; }
    }
}
