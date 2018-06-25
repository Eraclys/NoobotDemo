using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoobotTrial.Middleware.FlightFinder
{
    public interface IFlightFinderClient
    {
        Task<IEnumerable<Flight>> Find();
    }
}