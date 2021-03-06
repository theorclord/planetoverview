using PlanetOverview.PlanetComponents;
using System.Collections.Generic;

namespace PlanetOverview.Serialization
{
    public class PlanetData
    {
        // Identifiers
        public string TextID { get; set; }
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
        //public List<Stack> PlanetSpaceLocations { get; private set; }
        //public List<Tile> PlanetLandTiles { get; private set; }

        //public Dictionary<BuildQueueType, List<Unit>> BuildQueues { get; private set; }
    }
}
