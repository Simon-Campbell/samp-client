using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.WPF
{
    using Ninject;
    using SAMP.Client.Services;
    using SAMP.Client.WPF.ViewModels;

    public class ClientBootstrapper : Bootstrapper<MainViewModel>
    {
        IKernel _kernel;

        public ClientBootstrapper()
        {
            Start();
        }

        protected override void Configure()
        {
            _kernel = new StandardKernel(
                new ClientInjectionModule(),
                new ServicesInjectionModule());

            _kernel.Bind<IWindowManager>().To<MetroWindowManager>().InSingletonScope();
            _kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _kernel.Dispose();

            base.OnExit(sender, e);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            return _kernel.Get(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _kernel.GetAll(service);
        }

        protected override void BuildUp(object instance)
        {
            _kernel.Inject(instance);
        }
    }
}
