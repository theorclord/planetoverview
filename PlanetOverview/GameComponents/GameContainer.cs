using PlanetOverview.PlanetComponents;
using PlanetOverview.PlayerComponents;
using PlanetOverview.UnitComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

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

        /// <summary>
        /// Used for debug and development
        /// </summary>
        public void SaveJsonRepresentationOfFactions()
        {
            string filePath = AppContext.BaseDirectory + @"DataFiles\factions.json"; // TODO make this configurable (for mods)
            //Json
            string json = JsonSerializer.Serialize<List<Faction>>(Factions);

            using(TextWriter writer = new StreamWriter(filePath))
            {
                writer.Write(json);
            }

            string loadedJson = "";
            using(TextReader reader = new StreamReader(filePath))
            {
                loadedJson = reader.ReadToEnd();
            }

            List<Faction> testLoad = JsonSerializer.Deserialize<List<Faction>>(loadedJson);
        }

        public void LoadFactionsJson()
        {
            string loadedJson = "";
            string filePath = AppContext.BaseDirectory + @"DataFiles\factions.json"; // TODO make this configurable (for mods)
            using (TextReader reader = new StreamReader(filePath))
            {
                loadedJson = reader.ReadToEnd();
            }

            Factions = JsonSerializer.Deserialize<List<Faction>>(loadedJson);
        }


        /// <summary>
        /// Used for debugging and development
        /// </summary>
        /// <returns></returns>
        public string GenerateStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var p in AllPlanets)
            {
                builder.AppendLine(p.GetStringRep());
            }

            return builder.ToString();
        }
    }
}
