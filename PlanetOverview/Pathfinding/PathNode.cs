using PlanetOverview.PlanetComponents;

namespace PlanetOverview.Pathfinding.Pathfinding
{
    /// <summary>
    /// Holds the point in the path returned for the pathfinding algorithm
    /// </summary>
    public class PathNode
    {
        /// <summary>
        /// Cost of moving through this node
        /// </summary>
        public int Cost { get; set; }
        /// <summary>
        /// The planet on the path
        /// </summary>
        public Planet Planet { get; set; }
        public PathNode(int moveCost, Planet planet)
        {
            Cost = moveCost;
            Planet = planet;
        }
    }
}
