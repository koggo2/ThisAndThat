using System;
using System.Collections.Generic;
using TheTile.Util;
using UnityEngine;

namespace TheTile.Game
{
    public class AStarSearch
    {
        public Dictionary<Vector3Int, Vector3Int> CameFrom  = new Dictionary<Vector3Int, Vector3Int>();
        public Dictionary<Vector3Int, int> CostSoFar = new Dictionary<Vector3Int, int>();
        public List<Vector3Int> Path = new List<Vector3Int>();
        
        public static int Heuristic(Vector3Int a, Vector3Int b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public AStarSearch(WeightedGraph<Vector3Int> graph, Vector3Int start, Vector3Int destination)
        {
            var frontier = new PriorityQueue<Vector3Int>();
            frontier.Enqueue(start, 0);

            CameFrom[start] = start;
            CostSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(destination))
                {
                    Path.Add(current);
                    var rear = current;
                    
                    while (CameFrom.ContainsKey(rear) && CameFrom[rear] != rear)
                    {
                        Path.Add(CameFrom[rear]);
                        rear = CameFrom[rear];
                    }
                    break;
                }

                foreach (var next in graph.Neighbors(current))
                {
                    var newCost = CostSoFar[current] + graph.Cost(current, next);
                    
                    if(!CostSoFar.ContainsKey(next) || newCost < CostSoFar[next])
                    {
                        CostSoFar[next] = newCost;
                        var priority = newCost + Heuristic(next, destination);
                        frontier.Enqueue(next, priority);
                        CameFrom[next] = current;
                    }
                }
            }
        }
    }
}
