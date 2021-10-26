namespace PlanetOverview.UnitComponents
{
    /// <summary>
    /// Representation of units in game
    /// TODO split into space and land
    /// </summary>
    public class Unit
    {
        #region InstanceData
        // All properties in this region should be copied in a copy constructor
        // Static data
        public string TextID { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int BuildEffortCost { get; set; }

        // The TextID's of all the requirements needed to build this unit
        public string[] Requirements { get; set; }
        #endregion

        // variable data
        public int QueuedPrice { get; set; }
        public int BuildEffortRemaing { get; set; }
    }
}
