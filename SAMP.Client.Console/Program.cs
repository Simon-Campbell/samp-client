using SAMP.Client.Data.Queries.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var query = new ServerListQuery();

            var allFirst10 = query.All().Take(10);
            var hostedFirst10 = query.Where(x => x.IsHosted).Take(10);

            System.Console.WriteLine("First 10 Servers:");
            foreach (var server in allFirst10)
            {
                System.Console.WriteLine(String.Format("{0}:{1} (IsHosted = {2})", server.IpAddress, server.Port, server.IsHosted));
            }

            System.Console.WriteLine("First 10 Hosted Servers:");
            foreach (var server in hostedFirst10)
            {
                System.Console.WriteLine(String.Format("{0}:{1} (IsHosted = {2})", server.IpAddress, server.Port, server.IsHosted));
            }

            System.Console.Read();
        }
    }
}
