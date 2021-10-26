using static PlanetOverview.PlanetComponents.Planet;

namespace PlanetOverview.UnitComponents
{
    /// <summary>
    /// The structures on the ground planet
    /// This might be make sense to have as an composite class shared with space station for the shared variables
    /// </summary>
    public class Structure : Unit
    {
        // TODO change this to only effect specific queues
        public int BuildEffortProvided { get; set; }
        public BuildQueueType QueueEffortType { get; set; }

        /// <summary>
        /// Constructor for creating a new instance based on a base instance
        /// </summary>
        /// <param name="baseStruc"></param>
        public Structure(Structure baseStruc)
        {
            // general attributes
            TextID = baseStruc.TextID;
            Name = baseStruc.Name;
            Cost = baseStruc.Cost;
            BuildEffortCost = baseStruc.BuildEffortCost;
            Requirements = baseStruc.Requirements;

            BuildEffortProvided = baseStruc.BuildEffortProvided;
        }

        /// <summary>
        /// Used for deserialization
        /// </summary>
        public Structure() { }
    }
}
