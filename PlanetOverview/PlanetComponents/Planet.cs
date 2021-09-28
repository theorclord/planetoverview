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

        // Tiles
        public List<Tile> PlanetSpaceTiles { get; private set; }
        public List<Tile> PlanetLandTiles { get; private set; }

        // internal factors
        public int Income { get; set; }

        public Planet()
        {
            int pSize = Constants.PlanetTileSize;
            PlanetLandTiles = new List<Tile>(pSize);
            for(int i = 0; i< pSize; i++)
            {
                PlanetLandTiles.Add(new Tile());
            }

            int sSize = Constants.SpaceTileSize;
            PlanetSpaceTiles = new List<Tile>(sSize);
            for(int i = 0; i< sSize; i++)
            {
                PlanetSpaceTiles.Add(new Tile());
            }

            AdjacentPlanets = new List<Planet>();
            PlanetStructures = new List<Structure>();
        }

        public void AddLandUnit(Unit unit)
        {
            // find first land tile that is empty. Add the unit to the tile.
            var emptyTile = PlanetLandTiles.FirstOrDefault(t => t.Units.Count == 0);
            if(emptyTile == null)
            {
                // TODO error handling or add the unit to the space area
                return;
            }
            emptyTile.Units.Add(unit);
        }

        public void AddSpaceUnit(Unit unit)
        {
            PlanetSpaceTiles.First().Units.Add(unit);
        }
    }
}
