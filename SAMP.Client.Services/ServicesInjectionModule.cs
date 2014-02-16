using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Services
{
    public class ServicesInjectionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServerDiscoveryService>().To<ServerDiscoveryService>();
            Bind<IConfigurationService>().To<ConfigurationService>();
        }
    }
}
