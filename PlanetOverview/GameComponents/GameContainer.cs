using PlanetOverview.PlanetComponents;
using PlanetOverview.PlayerComponents;
using PlanetOverview.UnitComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetOverview.GameComponents
{
    public class GameContainer
    {
        public List<Planet> AllPlanets { get; set; }
        public List<Player> Players { get; set; }

        public List<Faction> Factions { get; set; }

        public GameContainer()
        {
            AllPlanets = new List<Planet>();
            Players = new List<Player>();
            Factions = new List<Faction>();
        }

        public void LoadGameData()
        {
            // Load all faction data from a text file
        }

        public string GenerateStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var p in AllPlanets)
            {
                string owner = p.Owner != null ? p.Owner.Name : "Neutral";
                builder.AppendLine($"Planet named, {p.Name}, owned by, {owner}.");
                builder.AppendLine($"The planets income is {p.Income}.");
                builder.AppendLine($"It supports level {p.SupportedSpaceStationLevel} space station, and {p.SupportedGroundStructureAmount} ground structures.");
                builder.AppendLine($"Located at ({p.Coords.X},{p.Coords.Y}) has the following neighbors:");
                
                // list all adjacent planets
                foreach (var ajp in p.AdjacentPlanets)
                {
                    builder.AppendLine($"Neighbor planet, {ajp.Name}");
                }
                builder.AppendLine();

                // List current strutures
                builder.AppendLine("Planet structures: ");
                foreach (var s in p.PlanetStructures)
                {
                    builder.AppendLine($"{s.Name}");
                }
                builder.AppendLine();

                // List current garison
                builder.AppendLine("Planet garrison:");
                foreach (var t in p.PlanetLandTiles)
                {
                    if (t.Units.Count > 0)
                    {
                        builder.AppendLine(t.Units.First().Name);
                    }
                }
                builder.AppendLine();

                // List current space station level / space structures
                if (p.PlanetSpaceStation != null)
                {
                    builder.AppendLine($"Space station level: {p.PlanetSpaceStation.Level}");
                }
                builder.AppendLine();

                builder.AppendLine("Current fleet:");
                // List current fleets
                foreach (var t in p.PlanetSpaceTiles)
                {
                    if (t.Units.Count > 0)
                    {
                        string fleet = string.Join(",", t.Units.Select(u => u.Name));
                        builder.AppendLine(fleet);
                    }
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
