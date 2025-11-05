using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");

            var lines = File.ReadAllLines(csvPath);
            if (lines.Length == 0)
            {
                logger.LogError("Error: There aren't any usable lines avaiable");
            }
            else if (lines.Length == 1)
            {
                logger.LogWarning("Warning: Only 1 line is usable, you need more.");
            }

            logger.LogInfo($"Lines: {lines[0]}");
            
            var parser = new TacoParser();
            
            var locations = lines.Select(line => parser.Parse(line)).ToArray();
            
            ITrackable tacoBell = null;
            ITrackable tacoBell2 = null;
            double distance = 0;

            for (int i = 0; i < locations.Length; i++)
            {
                var locA = locations[i];
                var corA = new GeoCoordinate();
                corA.Latitude = locA.Location.Latitude;
                corA.Longitude = locA.Location.Longitude;

                for (int x = 0; x < locations.Length; x++)
                {
                    var locB = locations[x];
                    var corB = new GeoCoordinate();
                    corB.Latitude = locB.Location.Latitude;
                    corB.Longitude = locB.Location.Longitude;

                    if (corA.GetDistanceTo(corB) > distance)
                    {
                        distance = corA.GetDistanceTo(corB);
                        tacoBell = locA;
                        tacoBell2 = locB;
                    }
                }
            }
            logger.LogInfo($"{tacoBell.Name}, and {tacoBell2.Name} have the greatest distance between them out of all the tacobells on this list.");
        }
    }
}
