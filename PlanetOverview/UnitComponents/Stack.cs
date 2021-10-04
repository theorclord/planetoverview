using System;
using System.Collections.Generic;
using System.Text;

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
