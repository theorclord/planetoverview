namespace Assets.Pathfinding.Pathfinding
{
    public class TerrainTile
    {
        public enum TerrainType
        {
            Road, Forest, Mountain, Castle, Bridge, Water, Grass
        }

        public enum MoveType
        {
            WaterPassable, Impassable, Normal, Flying, Transition
        }

        public TerrainType Type { get; set; }
        public MoveType MoveGroup { get; set; }
        public int Movecost { get; set; }

        public TerrainTile(TerrainType type)
        {
            Type = type;
            switch (type)
            {
                case TerrainType.Bridge:
                    Movecost = 1;
                    MoveGroup = MoveType.Transition;
                    break;
                case TerrainType.Castle:
                    Movecost = 1;
                    MoveGroup = MoveType.Normal;
                    break;
                case TerrainType.Forest:
                    Movecost = 2;
                    MoveGroup = MoveType.Flying;
                    break;
                case TerrainType.Grass:
                    Movecost = 2;
                    MoveGroup = MoveType.Normal;
                    break;
                case TerrainType.Mountain:
                    Movecost = 2;
                    MoveGroup = MoveType.Flying;
                    break;
                case TerrainType.Road:
                    Movecost = 1;
                    MoveGroup = MoveType.Transition;
                    break;
                case TerrainType.Water:
                    Movecost = 1;
                    MoveGroup = MoveType.WaterPassable;
                    break;
            }
        }
    }
}