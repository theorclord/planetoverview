using PlanetOverview.PlayerComponents;
using PlanetOverview.UnitComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetOverview.PlanetComponents
{
    /// <summary>
    /// This class containts all the data needed for the planet object
    /// </summary>
    public class Planet
    {
        public enum BuildQueueType
        {
            Land = 1, Space = 2, Structures = 3,
        }
        // Ideas to consider
        // it needs space station level 
        // Amount of planet structures
        // neighbors. this might be better to have in a general graph

        // build power => this is how much a planet can produce per turn. Define from structures or from planet
        // Build power comes in space, ground, maybe tech?


        // Identifiers
        public string Name { get; set; }
        
        public Location Coords { get; set; }
        public Player Owner { get; set; }

        // logic
        public int SupportedSpaceStationLevel { get; set; }
        public int SupportedGroundStructureAmount { get; set; }

        public List<Planet> AdjacentPlanets { get; set; }

        public List<Structure> PlanetStructures { get; set; }
        public SpaceStation PlanetSpaceStation { get; set; }

        // Unit locations
        /// <summary>
        /// Each planet have a number of space locations which contains the unit stack
        /// </summary>
        public List<Stack> PlanetSpaceLocations { get; private set; }
        public List<Tile> PlanetLandTiles { get; private set; }

        public int Income { get; set; }
        public int BaseBuildEffort { get; set; }

        public Dictionary<BuildQueueType,List<Unit>> BuildQueues { get; private set; }

        public Planet()
        {
            // Init planet tiles
            int pSize = Constants.PlanetTileSize;
            PlanetLandTiles = new List<Tile>(pSize);
            for(int i = 0; i< pSize; i++)
            {
                PlanetLandTiles.Add(new Tile());
            }

            // Init the stacks on the planet. 
            int sSize = Constants.SpaceTileSize;
            PlanetSpaceLocations = new List<Stack>(sSize);
            for(int i = 0; i< sSize; i++)
            {
                PlanetSpaceLocations.Add(new Stack());
            }

            AdjacentPlanets = new List<Planet>();
            PlanetStructures = new List<Structure>();

            BuildQueues = new Dictionary<BuildQueueType, List<Unit>>();
            foreach (BuildQueueType type in Enum.GetValues(typeof(BuildQueueType)).Cast<BuildQueueType>())
            {
                BuildQueues.Add(type, new List<Unit>());
            }
        }

        #region LocationFunctions
        /// <summary>
        /// Add a new unit to the planet. If the planet cannot contain the unit, send it to the space area.
        /// TODO: If the space area is occupied, unit is refunded.
        /// </summary>
        /// <param name="unit">The new land unit</param>
        public void AddNewLandUnit(Unit unit)
        {
            // find first land tile that is empty. Add the unit to the tile.
            var emptyTile = PlanetLandTiles.FirstOrDefault(t => t.Unit == null);
            if(emptyTile == null)
            {
                // try to add it to the space area
                AddNewUnitToSpaceArea(unit);
                return;
            }
            emptyTile.Unit = unit;
        }

        /// <summary>
        /// Used to add a newly produced unit to a planets space area.
        /// </summary>
        /// <param name="unit">The land or space unit</param>
        public void AddNewUnitToSpaceArea(Unit unit)
        {
            // Always add to the first stack on the planet. This includes both land and space units
            // each space stack can contain near unlimted amount of units, so just add it to the first
            PlanetSpaceLocations.First().Units.Add(unit);
        }

        /// <summary>
        /// Used for showing the units as a stack on a planet
        /// </summary>
        /// <returns></returns>
        public Stack GetGroundStack()
        {
            var unitStack = new Stack();
            foreach(var tile in PlanetLandTiles)
            {
                if(tile.Unit != null)
                {
                    unitStack.Units.Add(tile.Unit);
                }
            }
            return unitStack;
        }

        /// <summary>
        /// Adds a new structure to the planet. Does not take planet structure location into consideration
        /// </summary>
        /// <param name="struc"></param>
        public void AddNewStructure(Structure struc)
        {
            PlanetStructures.Add(struc);
        }
        #endregion

        #region buildRegion
        /// <summary>
        /// Adds a buildable to the build queue.
        /// Should not be callable if the unit queue is full.
        /// Should not be callable if you can't afford the buildable
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="queueType"></param>
        public void AddUnitToBuildQueue(Unit unit, BuildQueueType queueType)
        {
            //TODO figure out actual price for unit
            // this could be changed by price reduction an such
            unit.QueuedPrice = unit.Cost;
            unit.BuildEffortRemaing = unit.BuildEffortCost;

            Owner.Credits -= unit.QueuedPrice;

            if(BuildQueues[queueType].Count < Constants.BuildQueueLength)
            {
                BuildQueues[queueType].Add(unit);
            }
        }

        /// <summary>
        /// Method for removing a unit from a queue.
        /// This also refunds the price of the unit back to the owner.
        /// </summary>
        /// <param name="index">The index of the unit in the queue</param>
        /// <param name="queueType">The queue of the unit</param>
        public void RemoveUnitFromBuildQueue(int index, BuildQueueType queueType)
        {
            Unit unitToRemove = BuildQueues[queueType][index];
            BuildQueues[queueType].RemoveAt(index);
            Owner.Credits += unitToRemove.QueuedPrice;
        }

        public void ProgressBuildQueue(List<Unit> buildQueue, BuildQueueType type)
        {
            //TODO figure out how to spread the building out for each of the queues
            if (buildQueue.Count > 0)
            {
                
                int generatedEffortToSpend = BaseBuildEffort; // TODO Planet build value based on structures
                foreach (var struc in PlanetStructures)
                {
                    if(struc.QueueEffortType == type)
                    {
                        generatedEffortToSpend += struc.BuildEffortProvided;
                    }
                }

                while (generatedEffortToSpend > 0 && buildQueue.Count > 0)
                {
                    var currentProduction = buildQueue[0];

                    if (currentProduction.BuildEffortRemaing <= generatedEffortToSpend)
                    {
                        generatedEffortToSpend -= currentProduction.BuildEffortRemaing;
                        buildQueue.RemoveAt(0);
                        switch (type)
                        {
                            case BuildQueueType.Land:
                                AddNewLandUnit(currentProduction);
                                break;
                            case BuildQueueType.Space:
                                AddNewUnitToSpaceArea(currentProduction);
                                break;
                            case BuildQueueType.Structures:
                                AddNewStructure(currentProduction as Structure);
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                        
                        Console.WriteLine($"Debug: Unit produced at planet {Name}: {currentProduction.Name}");
                    }
                    else
                    {
                        currentProduction.BuildEffortRemaing -= generatedEffortToSpend;
                        generatedEffortToSpend = 0;
                    }
                }
            }
        }

        public List<Unit> GetBuildableUnits()
        {
            List<string> availableRequirements = GetAvailableRequirements();

            List<Unit> buildAbleUnits = new List<Unit>();
            foreach(Unit unit in Owner.Faction.Units)
            {
                bool buildAble = false;
                foreach(string id in unit.Requirements)
                {
                    buildAble = availableRequirements.Contains(id);
                }
                if(unit.Requirements == null || buildAble)
                {
                    buildAbleUnits.Add(unit);
                }
            }

            return buildAbleUnits;
        }

        public List<(bool, Structure)> GetBuildableStructures()
        {
            int ownerCredits = Owner.Credits;
            List<(bool, Structure)> buildableStrucs = new List<(bool, Structure)>();
            // if there is no space left, no structures can be built
            if (SupportedGroundStructureAmount == PlanetStructures.Count)
            {
                return buildableStrucs;
            }
            // Gather the requirements based on all requirement giving entities
            List<string> availableRequirements = GetAvailableRequirements();

            
            foreach (Structure struc in Owner.Faction.Structures)
            {
                bool buildAble = struc.Requirements == null;
                if (!buildAble)
                {
                    foreach (string id in struc.Requirements)
                    {
                        buildAble = availableRequirements.Contains(id);
                    }
                }
                if (buildAble)
                {
                    buildableStrucs.Add((ownerCredits >= struc.Cost, struc));
                }
            }

            return buildableStrucs;
        }

        private List<string> GetAvailableRequirements()
        {
            // Gather the requirements based on all requirement giving entities
            List<string> availableRequirements = new List<string>();
            foreach (Structure unit in PlanetStructures)
            {
                availableRequirements.Add(unit.TextID);
            }

            foreach(Unit unit in GetGroundStack().Units)
            {
                availableRequirements.Add(unit.TextID);
            }

            //TODO check the stack is the same as the owner
            foreach(Stack s in PlanetSpaceLocations)
            {
                foreach (Unit unit in s.Units)
                {
                    availableRequirements.Add(unit.TextID);
                }
            }
            
            return availableRequirements;
        }
        #endregion

        public void Progress()
        {
            // get income
            if (Owner != null)
            {
                //TODO have this depend on factors
                Owner.Credits += Income;
            }
            // progress queue
            // TODO run through all build type
            foreach(KeyValuePair<BuildQueueType,List<Unit>> buildQueue in BuildQueues)
            {
                ProgressBuildQueue(buildQueue.Value,buildQueue.Key);
            }
            // TODO ??
        }

        #region DebugAndTest
        public string GetStringRep()
        {
            StringBuilder builder = new StringBuilder();
            string owner = Owner != null ? Owner.Name : "Neutral";
            string ownerFaction = Owner != null ? Owner.Faction.Name : "Neutral Faction";
            builder.AppendLine($"Planet named, {Name}, owned by, {owner} of faction {ownerFaction}.");
            builder.AppendLine($"The planets income is {Income}.");
            builder.AppendLine($"It supports level {SupportedSpaceStationLevel} space station, and {SupportedGroundStructureAmount} ground structures.");
            builder.AppendLine($"Located at ({Coords.X},{Coords.Y}) has the following neighbors:");

            // list all adjacent planets
            foreach (var ajp in AdjacentPlanets)
            {
                builder.AppendLine($"Neighbor planet, {ajp.Name}");
            }
            builder.AppendLine();

            // List current strutures
            builder.AppendLine("Planet structures: ");
            foreach (var s in PlanetStructures)
            {
                builder.AppendLine($"{s.Name}");
            }
            builder.AppendLine();

            // List current garison
            builder.AppendLine("Planet garrison:");
            var unitGarrison = GetGroundStack();
            foreach (var u in unitGarrison.Units)
            {
                if (u != null)
                {
                    builder.AppendLine(u.Name);
                }
            }
            builder.AppendLine();

            // List current space station level / space structures
            if (PlanetSpaceStation != null)
            {
                builder.AppendLine($"Space station level: {PlanetSpaceStation.Level}");
            }
            builder.AppendLine();

            builder.AppendLine("Current fleet:");
            // List current fleets
            foreach (var stack in PlanetSpaceLocations)
            {
                if (stack.Units.Count > 0)
                {
                    string fleet = string.Join(",", stack.Units.Select(u => u.Name));
                    builder.AppendLine(fleet);
                }
            }
            builder.AppendLine();

            return builder.ToString();
        }
        #endregion
    }
}
