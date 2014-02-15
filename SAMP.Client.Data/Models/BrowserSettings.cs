using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Models
{
    public class BrowserSettings
    {
        public string DefaultUsername { get; set; }
        public IList<Server> FavouriteServers { get; set; }
    }
}
