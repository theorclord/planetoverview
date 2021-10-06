using PlanetOverview.UnitComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetOverview.PlayerComponents
{
    public class Player
    {
        public string Name { get; set; }
        public int Credits { get; set; }

        public Faction Faction { get; set; }
    }
}
