using Ninject.Modules;
using SAMP.Client.Data.Queries;
using SAMP.Client.Data.Queries.Implementations;
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
            Bind<IConfigurationQuery>().To<ConfigurationQuery>();
            Bind<IServerListQuery>().To<ServerListQuery>();
            Bind<IServerDetailsQuery>().To<ServerDetailsQuery>();
        }
    }
}
