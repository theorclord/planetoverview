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

            // Create factions with their structures and units
            container.LoadFactionsJson();


            // TODO load all this from files
            container.SaveJsonRepresentationOfFactions();

            // Create sample players
            Player p1 = new Player() { Name = "Player1", Faction = container.Factions[0], Credits = 10000 };
            container.Players.Add(p1);
            Player p2 = new Player() { Name = "Player2", Faction = container.Factions[1], Credits = 10000 };
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
            center.AddNewLandUnit(container.Players[0].Faction.Units[0]);
            center.AddNewLandUnit(container.Players[0].Faction.Units[0]);
            center.AddNewLandUnit(container.Players[0].Faction.Units[1]);
            center.PlanetSpaceStation = new SpaceStation() { Level = 3 };
            center.AddNewUnitToSpaceArea(container.Players[0].Faction.Units[2]);
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
            northSecondLeft.AddNewLandUnit(container.Players[1].Faction.Units[0]);
            northSecondLeft.AddNewUnitToSpaceArea(container.Players[1].Faction.Units[1]);
            northSecondLeft.AddNewUnitToSpaceArea(container.Players[1].Faction.Units[1]);
            northSecondLeft.AddNewUnitToSpaceArea(container.Players[1].Faction.Units[1]);
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
            southFirst.AddNewLandUnit(container.Players[1].Faction.Units[0]);
            southFirst.AddNewUnitToSpaceArea(container.Players[1].Faction.Units[1]);
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
            string rep = container.GenerateStringRepresentation();
            Console.Write(rep);
            Console.WriteLine("----------------------------------------------------------------------------------");

            //Build test
            center.AddUnitToBuildQueue(center.Owner.Faction.Units[0], Planet.BuildQueueType.Land);
            center.AddUnitToBuildQueue(center.Owner.Faction.Units[1], Planet.BuildQueueType.Land);
            center.AddUnitToBuildQueue(center.Owner.Faction.Units[0], Planet.BuildQueueType.Land);
            center.AddUnitToBuildQueue(center.Owner.Faction.Units[0], Planet.BuildQueueType.Land);

            // print the queue
            PrintUnitList(center.LandBuildQueue);

            center.RemoveUnitFromLandBuildQueue(2);

            Console.WriteLine();
            PrintUnitList(center.LandBuildQueue);
            center.RemoveUnitFromLandBuildQueue(2);
            Console.WriteLine();
            PrintUnitList(center.LandBuildQueue);

            center.AddUnitToBuildQueue(center.Owner.Faction.Units[0], Planet.BuildQueueType.Land);
            center.AddUnitToBuildQueue(center.Owner.Faction.Units[0], Planet.BuildQueueType.Land);

            RunCycle(container);
        }
        private static void PrintUnitList(List<Unit> unitList)
        {
            for(int i = 0; i< unitList.Count; i++)
            {
                Console.WriteLine(unitList[i].Name);
            }
        }

        private static void RunCycle(GameContainer container)
        {
            foreach(var p in container.AllPlanets)
            {
                p.Progress();
            }
        }
    }
}