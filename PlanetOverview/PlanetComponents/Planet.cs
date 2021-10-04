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
            Land = 1, Space = 2, 
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

        public List<Unit> SpaceBuildQueue { get; private set; }
        public List<Unit> LandBuildQueue { get; private set; }

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
            LandBuildQueue = new List<Unit>(Constants.BuildQueueLength);
            SpaceBuildQueue = new List<Unit>(Constants.BuildQueueLength);
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
            var emptyTile = PlanetLandTiles.FirstOrDefault(t => t.Unit != null);
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
                unitStack.Units.Add(tile.Unit);
            }
            return unitStack;
        }
        #endregion

        #region buildRegion
        /// <summary>
        /// Adds a unit to the build queue. 
        /// Will only be callable if there is less units in queue than queue length
        /// </summary>
        /// <param name="unit"></param>
        public void AddUnitToLandBuildQueue(Unit unit)
        {

            if(LandBuildQueue.Count < Constants.BuildQueueLength)
            {
                LandBuildQueue.Add(unit);
            }
        }

        /// <summary>
        /// Adds a buildable to the build queue.
        /// Should not be callable if the unit queue is full.
        /// Should not be callable if you can't afford the buildable
        /// </summary>
        /// <param name="buildable"></param>
        /// <param name="queueType"></param>
        public void AddUnitToBuildQueue(Unit unit, BuildQueueType queueType)
        {
            //TODO figure out actual price for unit
            // this could be changed by price reduction an such
            unit.QueuedPrice = unit.Cost;


            Owner.Credits -= unit.QueuedPrice;


            switch (queueType)
            {
                case BuildQueueType.Land:
                    if (LandBuildQueue.Count < Constants.BuildQueueLength)
                    {
                        LandBuildQueue.Add(unit);
                    }
                    break;
                case BuildQueueType.Space:
                    if (SpaceBuildQueue.Count < Constants.BuildQueueLength)
                    {
                        SpaceBuildQueue.Add(unit);
                    }
                    break;
                default:
                    break;
            }
        }


        public void RemoveUnitFromLandBuildQueue(int index)
        {
            var unitToRemove = LandBuildQueue[index];
            LandBuildQueue.RemoveAt(index);
            Owner.Credits += unitToRemove.QueuedPrice;
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

            // ??
        }

        public void ProgressBuildQueue()
        {

        }
    }
}
