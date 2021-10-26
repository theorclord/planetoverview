namespace Assets.Pathfinding.Pathfinding
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
        /// Location coordinates
        /// </summary>
        public int[] Coord { get; set; }
        public PathNode(int moveCost, int[] coordinate)
        {
            Cost = moveCost;
            Coord = coordinate;
        }
    }
}
