using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Services
{
    public class CachedPingService : IPingService
    {
        private IDictionary<string, CachedPingResponse> _pingCache = new ConcurrentDictionary<string, CachedPingResponse>();

        public int GetPing(string hostName)
        {
            if (_pingCache.ContainsKey(hostName))
            {
                var cacheEntry = _pingCache[hostName];

                if (!(DateTimeOffset.Now - cacheEntry.PingedTime > TimeSpan.FromSeconds(4)))
                {
                    return cacheEntry.ElapsedPingTime;
                }
                else
                {
                    return GetPingTask(hostName);
                }
            }
            else
            {
                return GetPingTask(hostName);
            }
        }

        private int GetPingTask(string hostName)
        {
            var pinger = new Ping();
            var stopwatch = new Stopwatch();

            var cachedPingResponse = new CachedPingResponse
            {
                PingedTime = DateTimeOffset.Now
            };

            _pingCache[hostName] = cachedPingResponse;

            stopwatch.Start();
            pinger.Send(hostName);
            stopwatch.Stop();

            cachedPingResponse.ElapsedPingTime = (int)stopwatch.ElapsedMilliseconds;
            return (int)stopwatch.ElapsedMilliseconds;
        }

        private class CachedPingResponse
        {
            public DateTimeOffset PingedTime { get; set; }
            public int ElapsedPingTime { get; set; }
        }
    }
}
