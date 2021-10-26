using System.Collections.Generic;
using System.Text;

namespace PlanetOverview.UnitComponents
{
    /// <summary>
    /// This class control each of the units and structures available for each faction
    /// </summary>
    public class Faction
    {
        public string TextID { get; set; }
        public string Name { get; set; }
        public List<Structure> Structures { get; set; }
        public List<Unit> Units { get; set; }

        public Faction()
        {
            Structures = new List<Structure>();
            Units = new List<Unit>();
        }

        public string GetStringRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Structures[0].Name);
            sb.AppendLine(Structures[0].BuildEffortCost.ToString());
            sb.AppendLine(Structures[0].BuildEffortProvided.ToString());
            sb.AppendLine(Structures[0].Cost.ToString());
            return sb.ToString();
        }
    }
}
