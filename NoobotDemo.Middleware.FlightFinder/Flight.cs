using System;

namespace NoobotDemo.Middleware.FlightFinder
{
    public class Flight
    {
        public DateTime Departure { get; set; }
        public DateTime Return { get; set; }
        public double Price { get; set; }
    }
}