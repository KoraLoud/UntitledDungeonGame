﻿//A* implementation help from https://www.redblobgames.com/pathfinding/a-star/introduction.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledDungeonGame.Resources.Game;

namespace UntitledDungeonGame.Resources
{
    public class AStarSearch
    {
        public Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        public Dictionary<Tile, double> costSoFar = new Dictionary<Tile, double>(); 

        static public double Heuristic(Tile a, Tile b)
        {
            //return Math.Abs(a.Transform.X - b.Transform.X) + Math.Abs(a.Transform.Y - b.Transform.Y);
            return Math.Abs(a.GridPosition.X - b.GridPosition.X) + Math.Abs(a.GridPosition.Y - b.GridPosition.Y);
        }

        private Tile[,] grid = SceneManager.CurrentDungeon.DungeonEntityGrid;

        public AStarSearch(Tile start, Tile goal)
        {
            var frontier = new PriorityQueue<Tile>();
            frontier.Enqueue(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if(current.Equals(goal))
                {
                    break;
                }

                foreach(var next in current.Neighbors())
                {
                    double newCost = costSoFar[current] + next.Cost;
                    if(!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        double priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

        }
    }
}
