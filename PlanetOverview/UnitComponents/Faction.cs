using PlanetOverview.PlanetComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetOverview.UnitComponents
{
    /// <summary>
    /// This class control each of the units and structures available for each faction
    /// </summary>
    public class Faction
    {
        public string Name { get; set; }
        public List<Structure> Structures { get; set; }
        public List<Unit> Units { get; set; }

        public Faction()
        {
            Structures = new List<Structure>();
            Units = new List<Unit>();
        }
    }
}
