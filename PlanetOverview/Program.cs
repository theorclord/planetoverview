using PlanetOverview.PlanetComponents;
using PlanetOverview.PlayerComponents;
using PlanetOverview.UnitComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetOverview
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating simple planet system");

            // Create sample players
            List<Player> players = new List<Player>();
            Player empire = new Player() { Name = "Empire" };
            players.Add(empire);
            Player rebellion = new Player() { Name = "Rebellion" };
            players.Add(new Player() { Name = "Rebellion" });
            //players.Add(new Player() { Name = "Neutral" }); try with neutral as null

            // Create structures
            Structure imperialBarracks = new Structure() { Name = "Imperial Barracks" };
            Structure imperialRefinery = new Structure() { Name = "Imperial Refinery" };
            Structure imperialFactory = new Structure() { Name = "Imperial Factory" };
            Structure rebellionTrainingCamp = new Structure() { Name = "Rebellion Training Camp" };
            Structure rebellionFactory = new Structure() { Name = "Rebellion Factory" };

            // Create units
            Unit stormTrooper = new Unit() { Name = "Storm Trooper" };
            Unit atSt = new Unit() { Name = "AT-ST" };

            Unit starDestroyer = new Unit() { Name = "Star destroyer" };
            
            Unit rebelSoldier = new Unit() { Name = "Rebel Trooper" };

            Unit xWing = new Unit() { Name = "X-Wing" };



            // create node system // TODO
            List<Planet> allPlanets = new List<Planet>();
            
            // Create Planets
            Planet center = new Planet() 
            { 
                Name = "Center",
                SupportedGroundStructureAmount = 5,
                SupportedSpaceStationLevel = 4,
                Coords = new Location() { X = 0, Y = 0},
                Owner = empire,
                Income = 800,
            };
            center.PlanetStructures.Add(imperialBarracks);
            center.PlanetStructures.Add(imperialFactory);
            center.PlanetStructures.Add(imperialRefinery);
            center.AddLandUnit(stormTrooper);
            center.AddLandUnit(stormTrooper);
            center.AddLandUnit(atSt);
            center.PlanetSpaceStation = new SpaceStation() { Level = 3 };
            center.AddSpaceUnit(starDestroyer);
            allPlanets.Add(center);
            Planet northFirst = new Planet()
            {
                Name = "North First",
                SupportedGroundStructureAmount = 3,
                SupportedSpaceStationLevel = 2,
                Coords = new Location() { X = 0, Y = 1 },
                Owner = null,
                Income = 250,
            };
            allPlanets.Add(northFirst);
            Planet northSecondLeft = new Planet()
            {
                Name = "North Second Left",
                SupportedGroundStructureAmount = 4,
                SupportedSpaceStationLevel = 1,
                Coords = new Location() { X = -1, Y = 2 },
                Owner = rebellion,
                Income = 300,
            };
            northSecondLeft.PlanetStructures.Add(rebellionTrainingCamp);
            northSecondLeft.AddLandUnit(rebelSoldier);
            northSecondLeft.AddSpaceUnit(xWing);
            northSecondLeft.AddSpaceUnit(xWing);
            northSecondLeft.AddSpaceUnit(xWing);
            northSecondLeft.PlanetSpaceStation = new SpaceStation() { Level = 1 };
            allPlanets.Add(northSecondLeft);
            Planet northSecondRight = new Planet()
            {
                Name = "North Second Right",
                SupportedGroundStructureAmount = 8,
                SupportedSpaceStationLevel = 3,
                Coords = new Location() { X = 1, Y = 2 },
                Owner = null,
                Income = 400,
            };
            allPlanets.Add(northSecondRight);
            Planet southFirst = new Planet()
            {
                Name = "South First",
                SupportedGroundStructureAmount = 6,
                SupportedSpaceStationLevel = 2,
                Coords = new Location() { X = 0, Y = -1 },
                Owner = rebellion,
                Income = 500,
            };
            southFirst.PlanetStructures.Add(rebellionFactory);
            southFirst.AddLandUnit(rebelSoldier);
            southFirst.AddSpaceUnit(xWing);
            southFirst.PlanetSpaceStation = new SpaceStation() { Level = 2 };
            allPlanets.Add(southFirst);

            // Add connections 
            // Todo move to a collection of connections. Adding the weight of the rute
            center.AdjacentPlanets.Add(northFirst);
            center.AdjacentPlanets.Add(southFirst);
            northFirst.AdjacentPlanets.Add(center);
            northFirst.AdjacentPlanets.Add(northSecondRight);
            northFirst.AdjacentPlanets.Add(northSecondLeft);
            northSecondRight.AdjacentPlanets.Add(northFirst);
            northSecondRight.AdjacentPlanets.Add(northSecondLeft);
            northSecondLeft.AdjacentPlanets.Add(northSecondRight);
            northSecondLeft.AdjacentPlanets.Add(northFirst);
            southFirst.AdjacentPlanets.Add(center);



            // print the connection
            foreach (var p in allPlanets)
            {
                string owner = p.Owner != null ? p.Owner.Name : "Neutral";
                StringBuilder consoleText = new StringBuilder();
                consoleText.AppendLine($"Planet named, {p.Name}, owned by, {owner}.");
                consoleText.AppendLine($"The planets income is {p.Income}.");
                consoleText.AppendLine($"It supports level {p.SupportedSpaceStationLevel} space station, and {p.SupportedGroundStructureAmount} ground structures.");
                consoleText.AppendLine($"Located at ({p.Coords.X},{p.Coords.Y}) has the following neighbors:");
                Console.Write(consoleText.ToString());
                // list all adjacent planets
                foreach ( var ajp in p.AdjacentPlanets)
                {
                    Console.WriteLine($"Neighbor planet, {ajp.Name}");
                }
                Console.WriteLine();

                // List current strutures
                Console.WriteLine("Planet structures: ");
                foreach(var s in p.PlanetStructures)
                {
                    Console.WriteLine($"{s.Name}");
                }
                Console.WriteLine();

                // List current garison
                Console.WriteLine("Planet garrison:");
                foreach(var t in p.PlanetLandTiles)
                {
                    if(t.Units.Count > 0)
                    {
                        Console.WriteLine(t.Units.First().Name);
                    }
                }
                Console.WriteLine();

                // List current space station level / space structures
                if(p.PlanetSpaceStation != null)
                {
                    Console.WriteLine($"Space station level: {p.PlanetSpaceStation.Level}");
                }
                Console.WriteLine();

                Console.WriteLine("Current fleet:");
                // List current fleets
                foreach(var t in p.PlanetSpaceTiles)
                {
                    if(t.Units.Count >0)
                    {
                        string fleet = string.Join(",", t.Units.Select(u => u.Name));
                        Console.WriteLine(fleet);
                    }
                }
                Console.WriteLine();
            }

        }
    }
}