using PlanetOverview.UnitComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetOverview.PlanetComponents
{
    /// <summary>
    /// Super class for the space to contain units from the game
    /// </summary>
    public class Tile
    {
        public List<Unit> Units { get; set; }

        public Tile()
        {
            Units = new List<Unit>();
        }
    }
}
