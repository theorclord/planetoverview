using System.Collections.Generic;

namespace PlanetOverview.UnitComponents
{
    // Represents the cluster of units on the galaxy map
    public class Stack
    {
        public List<Unit> Units { get; set; }

        public Stack()
        {
            Units = new List<Unit>();
        }
    }
}
