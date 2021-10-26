using System;
using System.Collections.Generic;
using Assets.Scripts.Utility;

namespace Assets.Pathfinding.Pathfinding
{
    /// <summary>
    /// Shortest path finding algorithm moving towards goal. Non exhaustive
    /// </summary>
    public class AStar
    {
        /// <summary>
        /// Gets the sum og the distance on both axis, not the diagonal
        /// </summary>
        /// <param name="posx1">First coordinate of the first position</param>
        /// <param name="posy1">Second coordinate of the first position</param>
        /// <param name="posx2">First coordinate of the second position</param>
        /// <param name="posy2">Second coordinate of the second position</param>
        /// <returns></returns>
        public static int ManhattanDistance(int posx1, int posy1, int posx2, int posy2)
        {
            return Math.Abs(posx1 - posx2) + Math.Abs(posy1 - posy2);
        }

        /// <summary>
        /// Does a given position exist in the array
        /// </summary>
        /// <param name="x">First index to be tested</param>
        /// <param name="y">Second index to be tested</param>
        /// <param name="array">Array to check if index are within bounds</param>
        /// <returns></returns>
        public static bool Exists(int x, int y, TerrainTile[,] array)
        {
            return x >= 0 && y >= 0 && x < array.GetLength(0) && y < array.GetLength(1);
        }

        /// <summary>
        /// Main method for getting the shortest path from one point to another
        /// </summary>
        /// <param name="terrainLayout">The given path matrix. Each field contains information on whether it is passable and the movement cost</param>
        /// <param name="obstructed">Grid of temporarily blocked tiles</param>
        /// <param name="startX">First start index</param>
        /// <param name="startY">Second start index</param>
        /// <param name="goalX">First target index</param>
        /// <param name="goalY">Second target index</param>
        /// <returns>Shortest path to target. Null if no goal could be found</returns>
        public static List<PathNode> ShortestPath(TerrainTile[,] terrainLayout, bool[,] obstructed, int startX, int startY, int goalX, int goalY)
        {
            // initialize the distance matrix for route comparison
            int[,] distanceMatrix = new int[terrainLayout.GetLength(0), terrainLayout.GetLength(1)];
            for (int i = 0; i < distanceMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < distanceMatrix.GetLength(1); j++)
                {
                    distanceMatrix[i, j] = -1;
                }
            }
            // Start point has distance 0
            distanceMatrix[startX, startY] = 0;

            // Add start node
            PriorityQueueMin<Node> openSet = new PriorityQueueMin<Node>();
            Node initial = new Node(startX, startY, 0, ManhattanDistance(startX, startY, goalX, goalY), terrainLayout[startX, startY].MoveGroup, terrainLayout[startX, startY].Movecost);
            openSet.Insert(initial);

            // run through the loop until the priority queue is empty
            while (!openSet.IsEmpty())
            {
                Node current = openSet.DelMin();
                // if we have reached the goal, get and return the path
                if (current.X == goalX && current.Y == goalY)
                {
                    List<PathNode> path = new List<PathNode>();
                    while (current.Parent != null)
                    {
                        path.Add(new PathNode(current.MoveCost, new int[] { current.X, current.Y }));
                        current = current.Parent;
                    }
                    path.Reverse();
                    return path;
                }

                // search all the neighbours
                List<Node> neighbours = current.GenerateNeighbours(terrainLayout, obstructed, distanceMatrix, goalX, goalY);
                openSet.InsertRange(neighbours);
            }
            // we failed to find the goal
            return null;
        }
    }

    #region Node
    /// <summary>
    /// Class for handling each node in the search
    /// </summary>
    public class Node : IComparable<Node>
    {
        private TerrainTile.MoveType Type { get; set; }
        public Node Parent { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        private int CostToGetHere { get; set; }
        private int EstimatedCostToGoal { get; set; }
        public int MoveCost { get; set; }

        public Node(int x, int y, int costToGetHere, int estimatedCostToGoal, TerrainTile.MoveType type, int moveCost)
        {
            X = x;
            Y = y;
            CostToGetHere = costToGetHere;
            EstimatedCostToGoal = estimatedCostToGoal;
            Type = type;
            MoveCost = moveCost;
        }

        /// <summary>
        /// Estimated Total cost for using this path
        /// </summary>
        /// <returns></returns>
        private int Total()
        {
            return CostToGetHere + EstimatedCostToGoal;
        }

        /// <summary>
        /// Used by the priority queue to order the nodes
        /// </summary>
        /// <param name="node">Node to compare to this node</param>
        /// <returns>0 if equal, -1 if less, 1 if higher</returns>
        public int CompareTo(Node node)
        {
            Node other = node;
            if (Total() == other.Total())
            {
                return 0;
            }
            else return (Total() < other.Total() ? -1 : 1);
        }

        /// <summary>
        /// Method for generating neighbors for the node. This method expects a grid layout and ands all the surrounding tiles
        /// </summary>
        /// <param name="terrainLayout">Grid to search through</param>
        /// <param name="obstructed">Temporarily blocked tiles</param>
        /// <param name="distanceMatrix">The current calculation of the cost to get to the tiles</param>
        /// <param name="goalX">Target first index</param>
        /// <param name="goalY">Target second index</param>
        /// <returns></returns>
        public List<Node> GenerateNeighbours(TerrainTile[,] terrainLayout, bool[,] obstructed, int[,] distanceMatrix, int goalX, int goalY)
        {
            List<Node> list = new List<Node>();
            CreateAndAdd(X + 1, Y, goalX, goalY, terrainLayout, obstructed, distanceMatrix, list);
            CreateAndAdd(X + 1, Y + 1, goalX, goalY, terrainLayout, obstructed, distanceMatrix, list);
            CreateAndAdd(X + 1, Y - 1, goalX, goalY, terrainLayout, obstructed, distanceMatrix, list);
            CreateAndAdd(X - 1, Y, goalX, goalY, terrainLayout, obstructed, distanceMatrix, list);
            CreateAndAdd(X - 1, Y - 1, goalX, goalY, terrainLayout, obstructed, distanceMatrix, list);
            CreateAndAdd(X - 1, Y + 1, goalX, goalY, terrainLayout, obstructed, distanceMatrix, list);
            CreateAndAdd(X, Y + 1, goalX, goalY, terrainLayout, obstructed, distanceMatrix, list);
            CreateAndAdd(X, Y - 1, goalX, goalY, terrainLayout, obstructed, distanceMatrix, list);

            return list;
        }

        /// <summary>
        /// Used for creating a node to add to the search algorithm if possible
        /// </summary>
        /// <param name="newX">The new node's first index</param>
        /// <param name="newY">The new node's second index</param>
        /// <param name="goalX">The target node's first index</param>
        /// <param name="goalY">The target node's second index</param>
        /// <param name="terrainLayout">The total grid</param>
        /// <param name="obstructed">Temporarily blocked tiles</param>
        /// <param name="distanceMatrix">The current calculation of the cost to get to the tiles</param>
        /// <param name="list">List passed for populating the resulting nodes</param>
        private void CreateAndAdd(int newX, int newY, int goalX, int goalY, TerrainTile[,] terrainLayout,
                                    bool[,] obstructed, int[,] distanceMatrix, List<Node> list)
        {
            // Checks for out of bounds
            if (AStar.Exists(newX, newY, terrainLayout))
            {
                bool passableTerrain = false;
                int newCost = CostToGetHere + terrainLayout[newX, newY].Movecost;
                int moveCost = terrainLayout[newX, newY].Movecost;
                // If the tile is impassable or currently obstructed, do not create a node
                if (Type == TerrainTile.MoveType.Impassable || terrainLayout[newX, newY].MoveGroup == TerrainTile.MoveType.Impassable ||
                  obstructed[newX, newY])
                {
                    passableTerrain = false;
                }
                else
                {
                    // checks for move restrictions between different types
                    switch (terrainLayout[newX, newY].MoveGroup)
                    {
                        case TerrainTile.MoveType.Flying:
                            // TODO check stack for passable, if stack has flying
                            passableTerrain = false;
                            break;
                        case TerrainTile.MoveType.Normal:
                            if (Type == TerrainTile.MoveType.Normal || Type == TerrainTile.MoveType.Transition)
                            {
                                passableTerrain = true;
                            }
                            else
                            {
                                passableTerrain = false;
                            }
                            break;
                        case TerrainTile.MoveType.Transition:
                            if (Type == TerrainTile.MoveType.Transition || Type == TerrainTile.MoveType.WaterPassable || Type == TerrainTile.MoveType.Normal)
                            {
                                passableTerrain = true;
                            }
                            else
                            {
                                passableTerrain = false;
                            }
                            break;
                        case TerrainTile.MoveType.WaterPassable:
                            if (Type == TerrainTile.MoveType.Transition || Type == TerrainTile.MoveType.WaterPassable)
                            {
                                passableTerrain = true;
                            }
                            else
                            {
                                passableTerrain = false;
                            }
                            break;
                    }
                }

                // if the tile is not passable, do not make a node
                if (passableTerrain)
                {
                    int newEstimate = AStar.ManhattanDistance(newX, newY, goalX, goalY);
                    Node newNode = new Node(newX, newY, newCost, newEstimate, terrainLayout[newX, newY].MoveGroup, moveCost)
                    {
                        Parent = this
                    };
                    if (distanceMatrix[newX, newY] < 0 || newCost < distanceMatrix[newX, newY])
                    {
                        list.Add(newNode);
                        distanceMatrix[newX, newY] = newCost;
                    }
                }
            }
        }
    }
    #endregion
}
