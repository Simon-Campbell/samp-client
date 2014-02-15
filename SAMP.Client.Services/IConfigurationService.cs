using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Services
{
    public interface IConfigurationService
    {
        BrowserSettings BrowserSettings { get; set; }
        IList<Server> ServerSettings { get; set; }
    }
}
