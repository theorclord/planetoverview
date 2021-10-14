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

        public string GetStringRepresentation()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Player: {Name}");
            sb.Append($"Credits: {Credits}");
            sb.Append($"Faction: {Faction.Name}");

            return sb.ToString();
        }
    }
}
