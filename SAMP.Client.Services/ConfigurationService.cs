using SAMP.Client.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAMP.Client.Services
{
    public class ConfigurationService : IConfigurationService
    {
        readonly IConfigurationQuery _configQuery;

        public ConfigurationService(IConfigurationQuery configQuery)
        {
            _configQuery = configQuery;
        }

        public Data.Models.BrowserSettings BrowserSettings
        {
            get
            {
                return _configQuery.GetBrowserSettings();
            }
        }

        public IList<Data.Models.Server> ServerSettings
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
