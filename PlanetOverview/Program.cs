using PlanetOverview.GameComponents;
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

            // Container holding all gamme information
            GameContainer container = new GameContainer();
            // Create sample players
            List<Player> players = new List<Player>();
            Player empire = new Player() { Name = "Empire", Credits = 10000 };
            players.Add(empire);
            Player rebellion = new Player() { Name = "Rebellion", Credits = 10000 };
            players.Add(rebellion);
            //players.Add(new Player() { Name = "Neutral" }); try with neutral as null

            // TODO load all this from files
            // Create factions with their structures and units
            Faction empire = new Faction()
            {
                Name = "Empire",
                Structures = new List<Structure>()
                {
                    new Structure() { Name = "Imperial Barracks" },
                    new Structure() { Name = "Imperial Refinery" },
                    new Structure() { Name = "Imperial Factory" },
                },
                Units = new List<Unit>()
                {
                    new Unit() 
                    { 
                        Name = "Storm Trooper",
                        BuildEffortCost = 50,
                        Cost = 50,
                    },
                    new Unit() { Name = "AT-ST",
                BuildEffortCost = 150,
                Cost = 150, },
                    new Unit() { Name = "Star destroyer",
                BuildEffortCost = 1000,
                Cost = 4000, },
                },
            };
            container.Factions.Add(empire);

            Faction rebellion = new Faction()
            {
                Name = "Rebellion",
                Structures = new List<Structure>()
                {
                    new Structure() { Name = "Rebellion Training Camp" },
                    new Structure() { Name = "Rebellion Factory" },
                },
                Units = new List<Unit>()
                {
                    new Unit() {
                Name = "Rebel Trooper",
                BuildEffortCost = 45,
                Cost = 40, },
                    new Unit() {  Name = "X-Wing",
                BuildEffortCost = 150,
                Cost = 150, },
                },
            };
            container.Factions.Add(rebellion);


            // Create sample players
            Player p1 = new Player() { Name = "Player1", Faction = empire };
            container.Players.Add(p1);
            Player p2 = new Player() { Name = "Player2", Faction = rebellion };
            container.Players.Add(p2);
            //players.Add(new Player() { Name = "Neutral" }); try with neutral as null on planets
            
            // Create Planets
            Planet center = new Planet() 
            { 
                Name = "Center",
                SupportedGroundStructureAmount = 5,
                SupportedSpaceStationLevel = 4,
                Coords = new Location() { X = 0, Y = 0},
                Owner = container.Players[0],
                Income = 800,
            };
            center.PlanetStructures.Add(container.Players[0].Faction.Structures[0]);
            center.PlanetStructures.Add(container.Players[0].Faction.Structures[2]);
            center.PlanetStructures.Add(container.Players[0].Faction.Structures[1]);
            center.AddLandUnit(container.Players[0].Faction.Units[0]);
            center.AddLandUnit(container.Players[0].Faction.Units[0]);
            center.AddLandUnit(container.Players[0].Faction.Units[1]);
            center.PlanetSpaceStation = new SpaceStation() { Level = 3 };
            center.AddSpaceUnit(container.Players[0].Faction.Units[2]);
            container.AllPlanets.Add(center);
            Planet northFirst = new Planet()
            {
                Name = "North First",
                SupportedGroundStructureAmount = 3,
                SupportedSpaceStationLevel = 2,
                Coords = new Location() { X = 0, Y = 1 },
                Owner = null,
                Income = 250,
            };
            container.AllPlanets.Add(northFirst);
            Planet northSecondLeft = new Planet()
            {
                Name = "North Second Left",
                SupportedGroundStructureAmount = 4,
                SupportedSpaceStationLevel = 1,
                Coords = new Location() { X = -1, Y = 2 },
                Owner = container.Players[1],
                Income = 300,
            };
            northSecondLeft.PlanetStructures.Add(container.Players[1].Faction.Structures[0]);
            northSecondLeft.AddLandUnit(container.Players[1].Faction.Units[0]);
            northSecondLeft.AddSpaceUnit(container.Players[1].Faction.Units[1]);
            northSecondLeft.AddSpaceUnit(container.Players[1].Faction.Units[1]);
            northSecondLeft.AddSpaceUnit(container.Players[1].Faction.Units[1]);
            northSecondLeft.PlanetSpaceStation = new SpaceStation() { Level = 1 };
            container.AllPlanets.Add(northSecondLeft);
            Planet northSecondRight = new Planet()
            {
                Name = "North Second Right",
                SupportedGroundStructureAmount = 8,
                SupportedSpaceStationLevel = 3,
                Coords = new Location() { X = 1, Y = 2 },
                Owner = null,
                Income = 400,
            };
            container.AllPlanets.Add(northSecondRight);
            Planet southFirst = new Planet()
            {
                Name = "South First",
                SupportedGroundStructureAmount = 6,
                SupportedSpaceStationLevel = 2,
                Coords = new Location() { X = 0, Y = -1 },
                Owner = container.Players[1],
                Income = 500,
            };
            southFirst.PlanetStructures.Add(container.Players[1].Faction.Structures[1]);
            southFirst.AddLandUnit(container.Players[1].Faction.Units[0]);
            southFirst.AddSpaceUnit(container.Players[1].Faction.Units[1]);
            southFirst.PlanetSpaceStation = new SpaceStation() { Level = 2 };
            container.AllPlanets.Add(southFirst);

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

            // print the setup
            Console.WriteLine("----------------------------------------------------------------------------------");
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
                var unitGarrison = p.GetGroundStack();
                foreach (var u in unitGarrison.Units)
                {
                    if(u != null) 
                    {
                        Console.WriteLine(u.Name);
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
                foreach(var stack in p.PlanetSpaceLocations)
                {
                    if(stack.Units.Count >0)
                    {
                        string fleet = string.Join(",", stack.Units.Select(u => u.Name));
                        Console.WriteLine(fleet);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("----------------------------------------------------------------------------------");

            //Build test
            center.AddUnitToLandBuildQueue(stormTrooper);
            center.AddUnitToLandBuildQueue(atSt);
            center.AddUnitToLandBuildQueue(stormTrooper);
            center.AddUnitToLandBuildQueue(stormTrooper);

            // print the queue
            PrintUnitList(center.LandBuildQueue);

            center.RemoveUnitFromLandBuildQueue(2);

            Console.WriteLine();
            PrintUnitList(center.LandBuildQueue);
            center.RemoveUnitFromLandBuildQueue(2);
            Console.WriteLine();
            PrintUnitList(center.LandBuildQueue);
        }
        private static void PrintUnitList(List<Unit> unitList)
        {
            for(int i = 0; i< unitList.Count; i++)
            {
                Console.WriteLine(unitList[i].Name);
            }
        }

        private static void RunCycle()
        {

        }
    }
}