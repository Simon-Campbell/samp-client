using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Queries.Implementations
{
    public class ConfigurationQuery : IConfigurationQuery
    {
        public Models.BrowserSettings GetBrowserSettings()
        {
            return BrowserSettings.Default;
        }

        public IList<Models.ServerSettings> GetFavouriteServerSettings()
        {
            throw new NotImplementedException();
        }
    }
}
