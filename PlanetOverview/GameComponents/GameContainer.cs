using PlanetOverview.PlanetComponents;
using PlanetOverview.PlayerComponents;
using PlanetOverview.Serialization;
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

        private List<Faction> Factions { get; set; }

        public Dictionary<string,Faction> IDFactions { get; set; }

        public GameContainer()
        {
            AllPlanets = new List<Planet>();
            Players = new List<Player>();
            Factions = new List<Faction>();

            IDFactions = new Dictionary<string, Faction>();
        }

        

        /// <summary>
        /// Loads the factions from the json file
        /// </summary>
        public void LoadFactionsJson()
        {
            string loadedJson = "";
            string filePath = AppContext.BaseDirectory + @"DataFiles\factions.json"; // TODO make this configurable (for mods)
            using (TextReader reader = new StreamReader(filePath))
            {
                loadedJson = reader.ReadToEnd();
            }

            Factions = JsonSerializer.Deserialize<List<Faction>>(loadedJson);
            foreach(var fac in Factions)
            {
                IDFactions.Add(fac.TextID, fac);
            }
        }

        /// <summary>
        /// TODO This function should be changed to load the galactic map
        /// </summary>
        public void LoadPlanets()
        {
            string loadedJson = "";
            string filePath = AppContext.BaseDirectory + @"DataFiles\planets.json"; // TODO make this configurable (for mods)
            using (TextReader reader = new StreamReader(filePath))
            {
                loadedJson = reader.ReadToEnd();
            }

            List<PlanetData> loadedData = JsonSerializer.Deserialize<List<PlanetData>>(loadedJson);
            // TODO add owner
            foreach(var pD in loadedData)
            {
                var tempPlanet = new Planet()
                {
                    Name = pD.Name,
                    Income = pD.BaseIncome,
                    BaseBuildEffort = pD.BaseBuildEffort,
                    SupportedGroundStructureAmount = pD.SupportedGroundStructureAmount,
                    SupportedSpaceStationLevel = pD.SupportedSpaceStationLevel,
                    Coords = pD.Coords,
                };
                if(pD.PlanetStructuresIDs != null)
                {
                    foreach (string strucId in pD.PlanetStructuresIDs)
                    {
                        tempPlanet.PlanetStructures.Add(
                            IDFactions[pD.FactionOwnerTextID].Structures.First(s => s.TextID == strucId)
                            );
                    }
                }
                AllPlanets.Add(tempPlanet);
            }
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

        /// <summary>
        /// Used for debug and development
        /// </summary>
        public void SaveJsonRepresentationGame()
        {
            // factions
            string filePathFactions = AppContext.BaseDirectory + @"DataFiles\factions.json"; // TODO make this configurable (for mods)
            string factionsJson = JsonSerializer.Serialize<List<Faction>>(Factions);
            using (TextWriter writer = new StreamWriter(filePathFactions))
            {
                writer.Write(factionsJson);
            }

            // planets
            string filePathPlanets = AppContext.BaseDirectory + @"DataFiles\planets.json"; // TODO make this configurable (for mods)
            string plantesJson = JsonSerializer.Serialize<List<Planet>>(AllPlanets);
            using (TextWriter writer = new StreamWriter(filePathPlanets))
            {
                writer.Write(plantesJson);
            }

            AllPlanets = JsonSerializer.Deserialize<List<Planet>>(plantesJson);
        }
    }
}
