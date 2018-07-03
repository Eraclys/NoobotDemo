using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoobotDemo.Middleware.FlightFinder
{
    public sealed class FlightFinderClient : IFlightFinderClient
    {
        private readonly Uri _flightUri;

        public FlightFinderClient(Uri baseUri)
        {
            _flightUri = new Uri(baseUri + "/api/Flights");
        }

        public async Task<IEnumerable<Flight>> Find()
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(_flightUri).ConfigureAwait(false);

                return JsonConvert.DeserializeObject<IEnumerable<Flight>>(content);
            }
        }
    }
}