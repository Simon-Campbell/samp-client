using Ninject.Modules;
using SAMP.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.WPF
{
    public class ClientInjectionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServerDiscoveryService>().To<ServerDiscoveryService>();
            Bind<IConfigurationService>().To<ConfigurationService>();
            Bind<IServerDetailsService>().To<ServerDetailsService>();
        }
    }
}
