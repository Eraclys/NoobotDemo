using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoobotDemo.Middleware.FlightFinder
{
    public interface IFlightFinderClient
    {
        Task<IEnumerable<Flight>> Find();
    }
}