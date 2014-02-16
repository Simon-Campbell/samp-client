using SAMP.Client.Data.Models;
using SAMP.Client.Data.Queries;
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
            var input = String.Empty;

            System.Console.WriteLine("Type 'list' to test query, or type 'config' to test config");

            switch (input = System.Console.ReadLine())
            {
                case "list":
                    TestListQuery();
                    break;
                case "config":
                    TestConfiguration();
                    break;
                case "info-query":
                    TestInformationQuery();
                    break;
                default:
                    System.Console.WriteLine("Unsure how to handle '{0}'", input);
                    break;
            }
            System.Console.Read();
        }

        private static void TestInformationQuery()
        {
            var query = new ServerDetailsQuery();

            query.GetDetails(new Server { Hostname = "littlewhiteys.co.uk", Port = 7778 });
        }

        static void TestListQuery()
        {
            var query = new ServerListQuery();

            var allFirst10 = query.All().Take(10);
            var hostedFirst10 = query.Where(x => x.IsHosted).Take(10);

            System.Console.WriteLine("First 10 Servers:");
            foreach (var server in allFirst10)
            {
                System.Console.WriteLine(String.Format("{0}:{1} (IsHosted = {2})", server.Hostname, server.Port, server.IsHosted));
            }

            System.Console.WriteLine("First 10 Hosted Servers:");
            foreach (var server in hostedFirst10)
            {
                System.Console.WriteLine(String.Format("{0}:{1} (IsHosted = {2})", server.Hostname, server.Port, server.IsHosted));
            }
        }

        static void TestConfiguration()
        {
            var configQuery = new ConfigurationQuery();
            var settings = configQuery.GetBrowserSettings();

            if (String.IsNullOrEmpty(settings.Username))
            {
                settings.Username = "Simon";
                System.Console.WriteLine("Set name to Simon");
            }
            else
            {
                System.Console.WriteLine("Read in name {0} from settings", settings.Username);
            }

            if (settings.FavouriteServers == null)
            {
                settings.FavouriteServers = new List<Server> 
                {
                    new Server { Hostname = "localhost", Port = 7777 }
                };

                System.Console.WriteLine("Added localhost to your favourite servers");
            }
            else
            {
                System.Console.WriteLine("Read in {0} favourites", settings.FavouriteServers.Count);
            }

            settings.Save();
        }
    }
}
