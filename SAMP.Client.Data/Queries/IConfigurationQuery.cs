using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Queries
{
    public interface IConfigurationQuery
    {
        IBrowserSettings GetSettings();
    }
}
