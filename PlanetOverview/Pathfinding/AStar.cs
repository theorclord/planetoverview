using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utility;
using PlanetOverview.PlanetComponents;

namespace PlanetOverview.Pathfinding.Pathfinding
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
        /// Main method for getting the shortest path from one point to another
        /// </summary>
        /// <param name="terrainLayout">The given path matrix. Each field contains information on whether it is passable and the movement cost</param>
        /// <param name="obstructed">Grid of temporarily blocked tiles</param>
        /// <param name="startX">First start index</param>
        /// <param name="startY">Second start index</param>
        /// <param name="goalX">First target index</param>
        /// <param name="goalY">Second target index</param>
        /// <returns>Shortest path to target. Null if no goal could be found</returns>
        public static List<PathNode> ShortestPath(Planet startingPlanet, Planet targetPlanet)
        {
            List<PathNode> distanceMatrix = new List<PathNode>();
            // Add start node
            PriorityQueueMin<Node> openSet = new PriorityQueueMin<Node>();
            Node initial = new Node(startingPlanet, 0, 
                ManhattanDistance(startingPlanet.Coords.X, startingPlanet.Coords.Y,
                targetPlanet.Coords.X, targetPlanet.Coords.Y));
            openSet.Insert(initial);

            // run through the loop until the priority queue is empty
            while (!openSet.IsEmpty())
            {
                Node current = openSet.DelMin();
                // if we have reached the goal, get and return the path
                if (current.Planet == targetPlanet)
                {
                    List<PathNode> path = new List<PathNode>();
                    while (current.Parent != null)
                    {
                        // TODO change cost to path between planets
                        path.Add(new PathNode(1, current.Planet));
                        current = current.Parent;
                    }
                    path.Reverse();
                    return path;
                }

                // search all the neighbours
                List<Node> neighbours = current.GenerateNeighbours(targetPlanet, distanceMatrix);
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
        public Planet Planet { get; set; }
        public Node Parent { get; set; }
        private int CostToGetHere { get; set; }
        private int EstimatedCostToGoal { get; set; }
        //public int MoveCost { get; set; }

        public Node(Planet planet, int costToGetHere, int estimatedCostToGoal)
        {
            Planet = planet;
            CostToGetHere = costToGetHere;
            EstimatedCostToGoal = estimatedCostToGoal;
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
        public List<Node> GenerateNeighbours(Planet targetPlanet, List<PathNode> distanceMatrix)
        {
            List<Node> list = new List<Node>();
            foreach(var p in Planet.AdjacentPlanets)
            {
                CreateAndAdd(p, targetPlanet, distanceMatrix, list);
            }

            return list;
        }

        /// <summary>
        /// Used for creating a node to add to the search algorithm if possible
        /// </summary>
        /// <param name="planet"></param>
        /// <param name="targetPlanet"></param>
        /// <param name="distanceMatrix">The current calculation of the cost to get to the tiles</param>
        /// <param name="list">List passed for populating the resulting nodes</param>
        private void CreateAndAdd(Planet planet, Planet targetPlanet, List<PathNode> distanceMatrix, List<Node> list)
        {
            // if the planet already is in the list, don't add
            // TODO change the weight of moving here
            int newCost = CostToGetHere + 1;

            // TODO check if you are capable of going through this planet's space
            int newEstimate = AStar.ManhattanDistance(planet.Coords.X, planet.Coords.Y,
                targetPlanet.Coords.X, targetPlanet.Coords.Y);
            var pathNode = distanceMatrix.FirstOrDefault(x => x.Planet.TextID == planet.TextID);
            Node newNode = new Node(planet, newCost, newEstimate)
            {
                Parent = this
            };
            if (pathNode != null)
            {
                if(newCost < pathNode.Cost)
                {
                    pathNode.Cost = newCost;
                    list.Add(newNode);
                }
            } else
            {
                distanceMatrix.Add(new PathNode(newCost, planet));
                list.Add(newNode);
            }
        }
    }
    #endregion
}
