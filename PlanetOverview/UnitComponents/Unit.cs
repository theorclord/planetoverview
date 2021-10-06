using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetOverview.UnitComponents
{
    /// <summary>
    /// Representation of units in game
    /// TODO split into space and land
    /// </summary>
    public class Unit
    {
        // Static data
        public string Name { get; set; }
        public int Cost { get; set; }
        public int BuildEffortCost { get; set; }


        // variable data
        public int QueuedPrice { get; set; }
        public int BuildEffortRemaing { get; set; }
    }
}
