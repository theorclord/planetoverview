using PlanetOverview.PlanetComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetOverview.Serialization
{
    public class PlanetData
    {
        // Identifiers
        public string Name { get; set; }
        public string FactionOwnerTextID { get; set; }
        public int BaseIncome { get; set; }
        public int BaseBuildEffort { get; set; }
        // logic
        public int SupportedSpaceStationLevel { get; set; }
        public int SupportedGroundStructureAmount { get; set; }

        // Lists
        public List<string> PlanetStructuresIDs { get; set; }

        // Shared classes
        public Location Coords { get; set; }

        //public List<Planet> AdjacentPlanets { get; set; }
        //public SpaceStation PlanetSpaceStation { get; set; }

        // Unit locations
        /// <summary>
        /// Each planet have a number of space locations which contains the unit stack
        /// </summary>
        //public List<Stack> PlanetSpaceLocations { get; private set; }
        //public List<Tile> PlanetLandTiles { get; private set; }



        //public Dictionary<BuildQueueType, List<Unit>> BuildQueues { get; private set; }
    }
}
